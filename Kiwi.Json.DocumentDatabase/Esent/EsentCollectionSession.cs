using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Fluesent;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.DocumentDatabase.Indexing;
using Kiwi.Json.JPath;
using Kiwi.Json.Untyped;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Json.DocumentDatabase.Esent
{
    public class EsentCollectionSession : ICollectionSession
    {
        IJsonFilterStrategy _jsonFilterStrategy = new FilterStrategy();
        private readonly string _collectionName;
        public IEsentDatabase Database { get; protected set; }
        private IEsentInstance _instance;
        private IEsentSession _session;
        private IEsentTransaction _transaction;

        public EsentCollectionSession(IEsentDatabase database, string collectionName)
        {
            _collectionName = collectionName;
            Database = database;
        }

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
            if (_instance != null)
            {
                _instance.Dispose();
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

        public void EnsureIndex(IndexDefinition definition)
        {
            EnsureSessionStarted();

            var collectionId = EnsureCollection();

            using (var indexTable = _transaction.OpenTable("Index"))
            {
                var cursor = indexTable.CreateCursor("IX_Index_CollectionId_JsonPath");
                var indexRecord = cursor
                    .ScanEq(Mappings.IndexRecordMapper, indexTable.CreateKey().Int64(collectionId).String(definition.JsonPath))
                    .FirstOrDefault();

                var jsonPath = JSON.ParseJsonPath(definition.JsonPath);

                var indexId = indexRecord != null ? 
                    indexRecord.IndexId : 
                    indexTable.CreateInsertRecord()
                        .Int64("CollectionId", collectionId)
                        .AddString("JsonPath", definition.JsonPath)
                        .AddString("JsonDefinition", JSON.Write(definition))
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
                            var indexValues = from documentMember in jsonPath.Evaluate(documentValue)
                                              from filterValue in _jsonFilterStrategy.GetFilterValues(documentMember)
                                              select JSON.Write(filterValue).ToLowerInvariant();

                            foreach (var indexValue in indexValues)
                            {
                                indexValueTable.CreateInsertRecord()
                                    .Int64("IndexId", indexId)
                                    .Int64("DocumentId", documentRecord.DocumentId)
                                    .AddString("Json", indexValue)
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

            using (var indexTable = _transaction.OpenTable("Index"))
            {
                var indexCursor = indexTable.CreateCursor(null);

                var indexValues =
                    (from indexRecord in indexCursor.Scan(Mappings.IndexRecordMapper)
                     let jsonPath = JSON.ParseJsonPath(indexRecord.JsonPath)
                     from filterMember in jsonPath.Evaluate(filter)
                     from filterValue in _jsonFilterStrategy.GetFilterValues(filterMember)

                     select new {IndexId = indexRecord.IndexId, Value = JSON.Write(filterValue).ToLowerInvariant()})
                        .ToLookup(o => o.IndexId, o => o.Value);

                HashSet<Int64> documentIds = null;
                if (indexValues.Any())
                {
                    using (var indexValueTable = _transaction.OpenTable("IndexValue"))
                    {
                        foreach (var group in indexValues)
                        {
                            var indexId = group.Key;

                            var documentIdsForIndex = new HashSet<Int64>();
                            foreach (var indexValue in group.Distinct().OrderBy(s => s))
                            {
                                var cursor = indexValueTable.CreateCursor("IX_IndexValue_IndexId_Json");
                                foreach (var documentIdRecord in cursor.ScanEq(Mappings.IndexValue_DocumentId_RecordMapper, indexValueTable.CreateKey().Int64(indexId).String(indexValue)))
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
                                return Enumerable.Empty<KeyValuePair<string, T>>();
                            }
                        }
                    }
                }

                var f = _jsonFilterStrategy.CreateFilter(filter);

                var found = new List<KeyValuePair<string,T>>();
                if (documentIds == null)
                {
                    using (var documentTable = _transaction.OpenTable("Document") )
                    {
                        foreach (var documentRecord in documentTable.CreateCursor(null).Scan(Mappings.DocumentRecordMapper))
                        {
                            var document = JSON.Read(documentRecord.DocumentJson);
                            if (f.Matches(document))
                            {
                                found.Add(new KeyValuePair<string, T>(documentRecord.DocumentKey, document.ToObject<T>()));
                            }
                        }
                    }
                }
                else if (documentIds.Count > 0)
                {
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
                            }
                            else
                            {
                                var document = JSON.Read(documentRecord.DocumentJson);
                                if (f.Matches(document))
                                {
                                    found.Add(new KeyValuePair<string, T>(documentRecord.DocumentKey, document.ToObject<T>()));
                                }
                            }
                        }
                    }
                }
                return found;
            }
        }

        public T Get<T>(string key)
        {
            EnsureSessionStarted();

            using (var documenTable = _transaction.OpenTable("Document"))
            {
                var cursor = documenTable.CreateCursor("IX_Document_DocumentKey");
                var document = cursor
                    .EnumerateEq(documenTable.CreateKey().String(key.ToLowerInvariant()))
                    .Select(i => cursor.ReadTo(new Mappings.DocumentRecord(), Mappings.DocumentRecordMapper))
                    .FirstOrDefault();

                return document == null ? default(T) : JSON.Read<T>(document.DocumentJson);
            }
        }

        public void Put(string key, IJsonValue document)
        {
            EnsureSessionStarted();

            var collectionId = EnsureCollection();

            Remove(key);

            using (var documentTable = _transaction.OpenTable("Document"))
            {
                var documentId = documentTable.CreateInsertRecord()
                    .Int64("CollectionId", collectionId)
                    .AddString("DocumentKey", key.ToLowerInvariant())
                    .AddString("DocumentJson", JSON.Write(document))
                    .InsertWithAutoIncrement64("DocumentId");

                // Get all indexes
                using (var indexTable = _transaction.OpenTable("Index"))
                {
                    using (var indexValueTable = _transaction.OpenTable("IndexValue"))
                    {
                        var indexCursor = indexTable.CreateCursor("IX_Index_CollectionId");
                        foreach ( var indexRecord in indexCursor.Scan(Mappings.IndexRecordMapper))
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
                                    .AddString("Json", indexValue)
                                    .Insert();
                            }
                        }

                    }
                }
            }
        }

        public void Remove(string key)
        {
            using (var documentTable = _transaction.OpenTable("Document"))
            {

                // Find existing document
                var documentCursor = documentTable.CreateCursor("IX_Document_DocumentKey");

                var documentKey = documentTable.CreateKey().String("key");
                var documentRecord = documentCursor.EnumerateEq(documentKey)
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

        private Int64 EnsureCollection()
        {
            using (var table = _transaction.OpenTable("Collection"))
            {
                var cursor = table.CreateCursor("IX_Collection_CollectionName");

                var collectionRecord = cursor
                    .EnumerateEq(table.CreateKey().String(_collectionName))
                    .Select(i => cursor.ReadTo(new Mappings.CollectionRecord(), Mappings.CollectionRecordMapper))
                    .FirstOrDefault();

                if (collectionRecord != null)
                {
                    return collectionRecord.CollectionId;
                }

                return table.CreateInsertRecord()
                    .AddString("CollectionName", _collectionName)
                    .InsertWithAutoIncrement64("CollectionId");
            }
        }

        private Int64? GetCollectionid()
        {
            using (var table = _transaction.OpenTable("Collection"))
            {
                var cursor = table.CreateCursor("IX_Collection_CollectionName");
                var collectionRecord = cursor
                    .EnumerateEq(table.CreateKey().String(_collectionName))
                    .Select(i => cursor.ReadTo(new Mappings.CollectionRecord(), Mappings.CollectionRecordMapper))
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
            if (_instance == null)
            {
                _instance = Database.CreateInstance();
            }
            if (_session == null)
            {
                _session = _instance.CreateSession();
            }
            if (_transaction == null)
            {
                _transaction = _session.CreateTransaction();
            }
        }

    }
}