using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Fluesent;
using Kiwi.Json.JPath;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Esent
{
    public class DocumentCollectionIndex: IDocumentCollectionIndex
    {
        private readonly IEsentDatabase _database;
        private readonly string _collectionName;
        public IJsonPath JsonPath { get; set; }

        public DocumentCollectionIndex(IEsentDatabase database, string collectionName)
        {
            _database = database;
            _collectionName = collectionName;
        }


        public IEnumerable<IJsonValue> GetValues(string key)
        {
            var serializedValues = new List<string>();

            using (var session = _database.CreateSession(true))
            {
                using (var transaction = session.CreateTransaction())
                {
                    using (var collectionTable = transaction.OpenTable("Collection"))
                    {
                        var collection = collectionTable.CreateCursor("IX_Collection_CollectionName")
                            .ScanEq(Mappings.CollectionRecordMapper, collectionTable.CreateKey().String(_collectionName))
                            .FirstOrDefault();

                        if (collection == null)
                        {
                            return Enumerable.Empty<IJsonValue>();
                        }
                        using (var indexTable = transaction.OpenTable("Index"))
                        {
                            var indexRecord = indexTable.CreateCursor("IX_Index_CollectionId_JsonPath")
                                .ScanEq(Mappings.IndexRecordMapper,
                                        indexTable.CreateKey().Int64(collection.CollectionId).String(JsonPath.Path))
                                .FirstOrDefault();

                            if (indexRecord == null)
                            {
                                return Enumerable.Empty<IJsonValue>();
                            }
                            using (var documentTable = transaction.OpenTable("Document"))
                            {
                                var documentRecord = documentTable.CreateCursor("IX_Document_DocumentKey")
                                    .ScanEq(Mappings.Document_Id_RecordMapper, documentTable.CreateKey().String(key))
                                    .FirstOrDefault();
                                if (documentRecord == null)
                                {
                                    return Enumerable.Empty<IJsonValue>();
                                }

                                using (var indexValueTable = transaction.OpenTable("IndexValue"))
                                {
                                    serializedValues = indexValueTable.CreateCursor("PK_IndexValue_IndexId_DocumentId")
                                        .ScanEq(Mappings.IndexValue_Json_RecordMapper,
                                                indexValueTable.CreateKey().Int64(indexRecord.IndexId).Int64(
                                                    documentRecord.DocumentId))
                                        .Select(r => r.Json)
                                        .ToList();
                                }
                            }
                        }
                    }
                }
            }
            return (from json in serializedValues
                    select JsonConvert.Read<IJsonValue>(json))
                    .ToList();
        }

        public IEnumerable<KeyValuePair<string, IJsonValue>> GetValues()
        {
            var serializedValues = new List<Tuple<string, string>>();

            using (var session = _database.CreateSession(true))
            {
                using (var transaction = session.CreateTransaction())
                {
                    using (var collectionTable = transaction.OpenTable("Collection"))
                    {
                        var collection = collectionTable.CreateCursor("IX_Collection_CollectionName")
                            .ScanEq(Mappings.CollectionRecordMapper, collectionTable.CreateKey().String(_collectionName))
                            .FirstOrDefault();

                        if (collection == null)
                        {
                            return Enumerable.Empty<KeyValuePair<string,IJsonValue>>();
                        }
                        using (var indexTable = transaction.OpenTable("Index"))
                        {
                            var indexRecord = indexTable.CreateCursor("IX_Index_CollectionId_JsonPath")
                                .ScanEq(Mappings.IndexRecordMapper,indexTable.CreateKey().Int64(collection.CollectionId).String(JsonPath.Path))
                                .FirstOrDefault();

                            if (indexRecord == null)
                            {
                                return Enumerable.Empty<KeyValuePair<string, IJsonValue>>();    
                            }

                            using (var indexValueTable = transaction.OpenTable("IndexValue"))
                            {
                                using (var documentTable = transaction.OpenTable("Document"))
                                {
                                    var indexValues = indexValueTable.CreateCursor("IX_IndexValue_IndexId")
                                        .ScanEq(Mappings.IndexValueRecordMapper,
                                                indexValueTable.CreateKey().Int64(indexRecord.IndexId)).ToList();

                                    foreach (var indexValueRecord in indexValues)
                                    {
                                        var documentRecord = documentTable.CreateCursor("PK_Doucment_DocumentId")
                                            .ScanEq(Mappings.Document_Key_RecordMapper,
                                                    documentTable.CreateKey().Int64(indexValueRecord.DocumentId))
                                            .FirstOrDefault();

                                        if (documentRecord != null)
                                        {
                                            serializedValues.Add(Tuple.Create(documentRecord.DocumentKey, indexValueRecord.Json));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            return (from serializedValue in serializedValues
                    select
                        new KeyValuePair<string, IJsonValue>(serializedValue.Item1,
                                                             JsonConvert.Read<IJsonValue>(serializedValue.Item2)))
                .ToList();
        }
    }
}