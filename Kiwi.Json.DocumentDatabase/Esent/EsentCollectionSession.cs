using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Kiwi.Fluesent;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.DocumentDatabase.Indexing;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Esent
{
    public class EsentCollectionSession : ICollectionSession
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly string _collectionName;
        private readonly IJsonFilterStrategy _jsonFilterStrategy = new FilterStrategy();
        private IEsentSession _session;
        private IEsentTransaction _transaction;

        public EsentCollectionSession(IEsentDatabase database, string collectionName)
        {
            _collectionName = collectionName;
            Database = database;
        }

        public IEsentDatabase Database { get; protected set; }

        #region ICollectionSession Members

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            if (_session != null)
            {
                _session.Dispose();
            }
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public void Pulse()
        {
            if (_transaction != null)
            {
                _transaction.Pulse();
            }
        }

        public void EnsureIndex(IndexDefinition definition)
        {
            EnsureSessionStarted();

            _session.LockWrites();

            var collectionId = EnsureCollection();

            using (var indexTable = _transaction.OpenTable("Index"))
            {
                var indexRecord = indexTable.CreateCursor("IX_Index_CollectionId_JsonPath")
                    .ScanEq(Mappings.IndexRecordMapper,
                            indexTable.CreateKey().Int64(collectionId).String(definition.JsonPath.Path))
                    .FirstOrDefault();

                if (indexRecord != null)
                {
                    Log.TraceFormat("EnsureIndex({0}) has nothing to do. Already present.", definition.JsonPath);
                    return;
                }

                Log.TraceFormat("EnsureIndex({0}): Creating index.", definition.JsonPath);

                var indexId = indexTable.CreateInsertRecord()
                    .Int64("CollectionId", collectionId)
                    .String("JsonPath", definition.JsonPath.Path)
                    .String("JsonDefinition", "") // TODO: Save definition in suitable format
                    .InsertWithAutoIncrement64("IndexId");

                // Reindex all objects
                using (var documentTable = _transaction.OpenTable("Document"))
                {
                    using (var indexValueTable = _transaction.OpenTable("IndexValue"))
                    {
                        var documentCursor = documentTable.CreateCursor(null);

                        foreach (var documentRecord in documentCursor.Scan(Mappings.DocumentRecordMapper))
                        {
                            // Clear existing index values
                            indexValueTable.CreateCursor("PK_IndexValue_IndexId_DocumentId")
                                .DeleteEq(indexValueTable.CreateKey().Int64(indexId).Int64(documentRecord.DocumentId));

                            var documentValue = JSON.Read<IJsonValue>(documentRecord.DocumentJson);
                            var indexValues = from documentMember in definition.JsonPath.Evaluate(documentValue)
                                              from filterValue in _jsonFilterStrategy.GetFilterValues(documentMember)
                                              select JSON.Write(filterValue).ToLowerInvariant();

                            foreach (var indexValue in indexValues)
                            {
                                indexValueTable.CreateInsertRecord()
                                    .Int64("IndexId", indexId)
                                    .Int64("DocumentId", documentRecord.DocumentId)
                                    .String("Json", indexValue)
                                    .Insert();
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<KeyValuePair<string, T>> Find<T>(IJsonValue filter)
        {
            EnsureSessionStarted();

            var collectionId = GetCollectionid();
            if (collectionId == null)
            {
                return Enumerable.Empty<KeyValuePair<string, T>>();
            }

            var indexValues = GetIndexValuesByFindFilterGroupedByIndexId<T>(filter);

            if (indexValues.Any())
            {
                var documentIds = FindUsingIndex(indexValues);
                if (documentIds == null)
                {
                    return Enumerable.Empty<KeyValuePair<string, T>>();
                }

                return LoadObjectsFiltered<T>(documentIds, _jsonFilterStrategy.CreateFilter(filter));
            }

            Log.WarnFormat("Find({0}): No suitable indexes, scanning table", filter);
            return ScanObjectsFiltered<T>(_jsonFilterStrategy.CreateFilter(filter));
        }

        public T Get<T>(string key)
        {
            EnsureSessionStarted();

            using (var documenTable = _transaction.OpenTable("Document"))
            {
                var document = documenTable.CreateCursor("IX_Document_DocumentKey")
                    .ScanEq(Mappings.DocumentRecordMapper, documenTable.CreateKey().String(key.ToLowerInvariant()))
                    .FirstOrDefault();

                return document == null ? default(T) : JSON.Read<T>(document.DocumentJson);
            }
        }

        public void Put(string key, IJsonValue document)
        {
            EnsureSessionStarted();

            _session.LockWrites();

            var collectionId = EnsureCollection();

            Remove(key);

            using (var documentTable = _transaction.OpenTable("Document"))
            {
                var documentId = documentTable.CreateInsertRecord()
                    .Int64("CollectionId", collectionId)
                    .String("DocumentKey", key.ToLowerInvariant())
                    .String("DocumentJson", JSON.Write(document))
                    .InsertWithAutoIncrement64("DocumentId");

                // Get all indexes
                using (var indexTable = _transaction.OpenTable("Index"))
                {
                    using (var indexValueTable = _transaction.OpenTable("IndexValue"))
                    {
                        var indexCursor = indexTable.CreateCursor("IX_Index_CollectionId");
                        foreach (var indexRecord in indexCursor.Scan(Mappings.IndexRecordMapper))
                        {
                            var jsonPath = JSON.ParseJsonPath(indexRecord.JsonPath);

                            var indexValues = from documentMember in jsonPath.Evaluate(document)
                                              from filterValue in _jsonFilterStrategy.GetFilterValues(documentMember)
                                              select JSON.Write(filterValue).ToLowerInvariant();

                            foreach (var indexValue in indexValues)
                            {
                                indexValueTable.CreateInsertRecord()
                                    .Int64("IndexId", indexRecord.IndexId)
                                    .Int64("DocumentId", documentId)
                                    .String("Json", indexValue)
                                    .Insert();
                            }
                        }
                    }
                }
            }
        }

        public void Remove(string key)
        {
            EnsureSessionStarted();

            _session.LockWrites();

            using (var documentTable = _transaction.OpenTable("Document"))
            {
                // Find existing document
                var documentCursor = documentTable.CreateCursor("IX_Document_DocumentKey");

                var documentKey = documentTable.CreateKey().String("key");
                var documentRecord = documentCursor.ScanEq(Mappings.DocumentRecordMapper, documentKey)
                    .Select(i => documentCursor.ReadTo(new Mappings.DocumentRecord(), Mappings.DocumentRecordMapper))
                    .FirstOrDefault();

                if (documentRecord != null)
                {
                    // Delete the document
                    documentCursor.Delete();

                    // Delete all indexvalues for that document
                    using (var indexValueTable = _transaction.OpenTable("IndexValue"))
                    {
                        indexValueTable
                            .CreateCursor("IX_IndexValue_DocumentId")
                            .DeleteEq(indexValueTable.CreateKey().Int64(documentRecord.DocumentId));
                    }
                }
            }
        }

        #endregion

        private ILookup<Int64, string> GetIndexValuesByFindFilterGroupedByIndexId<T>(IJsonValue filter)
        {
            using (var indexTable = _transaction.OpenTable("Index"))
            {
                var indexCursor = indexTable.CreateCursor(null);

                return
                    (from indexRecord in indexCursor.Scan(Mappings.IndexRecordMapper)
                     let jsonPath = JSON.ParseJsonPath(indexRecord.JsonPath)
                     from filterMember in jsonPath.Evaluate(filter)
                     from filterValue in _jsonFilterStrategy.GetFilterValues(filterMember)
                     select new {indexRecord.IndexId, Value = JSON.Write(filterValue).ToLowerInvariant()})
                        .ToLookup(o => o.IndexId, o => o.Value);
            }
        }

        private List<KeyValuePair<string, T>> ScanObjectsFiltered<T>(IJsonFilter filter)
        {
            using (var documentTable = _transaction.OpenTable("Document"))
            {
                return new List<KeyValuePair<string, T>>(
                    from documentRecord in documentTable.CreateCursor(null).Scan(Mappings.DocumentRecordMapper)
                    let document = JSON.Read(documentRecord.DocumentJson)
                    where filter.Matches(document)
                    select new KeyValuePair<string, T>(documentRecord.DocumentKey, document.ToObject<T>()));
            }
        }

        private List<KeyValuePair<string, T>> LoadObjectsFiltered<T>(IEnumerable<long> documentIds, IJsonFilter filter)
        {
            var found = new List<KeyValuePair<string, T>>();

            using (var documentTable = _transaction.OpenTable("Document"))
            {
                foreach (var documentId in documentIds.OrderBy(id => id))
                {
                    var documentRecord =
                        documentTable.CreateCursor("PK_Doucment_DocumentId").ScanEq(
                            Mappings.DocumentRecordMapper, documentTable.CreateKey().Int64(documentId)).
                            FirstOrDefault();
                    if (documentRecord == null)
                    {
                        // TODO: Really bad incosistency
                        Log.ErrorFormat("Database is inconsistent - object id from index is not valid");
                    }
                    else
                    {
                        var document = JSON.Read(documentRecord.DocumentJson);
                        if (filter.Matches(document))
                        {
                            found.Add(new KeyValuePair<string, T>(documentRecord.DocumentKey,
                                                                  document.ToObject<T>()));
                        }
                    }
                }
            }
            return found;
        }

        private HashSet<Int64> FindUsingIndex(IEnumerable<IGrouping<Int64, string>> indexIdToIndexValues)
        {
            HashSet<Int64> documentIds = null;
            using (var indexValueTable = _transaction.OpenTable("IndexValue"))
            {
                foreach (var group in indexIdToIndexValues)
                {
                    var indexId = @group.Key;

                    var documentIdsForIndex = new HashSet<Int64>();
                    foreach (var indexValue in @group.Distinct().OrderBy(s => s))
                    {
                        var cursor = indexValueTable.CreateCursor("IX_IndexValue_IndexId_Json");
                        foreach (
                            var documentIdRecord in
                                cursor.ScanEq(Mappings.IndexValue_DocumentId_RecordMapper,
                                              indexValueTable.CreateKey().Int64(indexId).String(indexValue)))
                        {
                            documentIdsForIndex.Add(documentIdRecord.DocumentId);
                        }
                    }
                    if (documentIds == null)
                    {
                        documentIds = documentIdsForIndex;
                    }
                    else
                    {
                        documentIds.IntersectWith(documentIdsForIndex);
                    }
                    if (documentIds.Count == 0)
                    {
                        return null;
                    }
                }
            }
            return documentIds;
        }

        private Int64 EnsureCollection()
        {
            using (var table = _transaction.OpenTable("Collection"))
            {
                var cursor = table.CreateCursor("IX_Collection_CollectionName");

                var collectionRecord = cursor
                    .ScanEq(Mappings.CollectionRecordMapper, table.CreateKey().String(_collectionName))
                    .FirstOrDefault();

                if (collectionRecord != null)
                {
                    return collectionRecord.CollectionId;
                }

                return table.CreateInsertRecord()
                    .String("CollectionName", _collectionName)
                    .InsertWithAutoIncrement64("CollectionId");
            }
        }

        private Int64? GetCollectionid()
        {
            using (var table = _transaction.OpenTable("Collection"))
            {
                var collectionRecord = table.CreateCursor("IX_Collection_CollectionName")
                    .ScanEq(Mappings.CollectionRecordMapper, table.CreateKey().String(_collectionName))
                    .FirstOrDefault();

                if (collectionRecord != null)
                {
                    return collectionRecord.CollectionId;
                }
            }
            return null;
        }

        private void EnsureSessionStarted()
        {
            if (_session == null)
            {
                _session = Database.CreateSession(true);
            }
            if (_transaction == null)
            {
                _transaction = _session.CreateTransaction();
            }
        }
    }
}