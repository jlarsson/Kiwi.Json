using System.Collections.Generic;
using System.Linq;
using Kiwi.Fluesent;

namespace Kiwi.Json.DocumentDatabase.Esent
{
    public class EsentDocumentCollection : IDocumentCollection
    {
        public IEsentDatabase Database { get; protected set; }

        public EsentDocumentCollection(IEsentDatabase database, string name)
        {
            Database = database;
            Name = name;
        }

        public string Name { get; private set; }

        public ICollectionSession CreateSession()
        {
            return CreateCollectionSession();
        }

        private ICollectionSession CreateCollectionSession()
        {
            return new EsentCollectionSession(Database, Name);
        }

        public IEnumerable<IDocumentCollectionIndex> GetIndexes()
        {
            using (var session = Database.CreateSession(true))
            {
                using (var transaction = session.CreateTransaction())
                {
                    using (var collectionTable = transaction.OpenTable("Collection"))
                    {
                        var collection = collectionTable.CreateCursor("IX_Collection_CollectionName")
                            .ScanEq(Mappings.CollectionRecordMapper, collectionTable.CreateKey().String(Name))
                            .FirstOrDefault();

                        if (collection == null)
                        {
                            return Enumerable.Empty<IDocumentCollectionIndex>();
                        }
                        using (var indexTable = transaction.OpenTable("Index"))
                        {
                            return indexTable.CreateCursor("IX_Index_CollectionId")
                                .ScanEq(Mappings.IndexRecordMapper,
                                        indexTable.CreateKey().Int64(collection.CollectionId))
                                .Select(r => new DocumentCollectionIndex(Database, Name)
                                                 {
                                                     JsonPath = JsonConvert.ParseJsonPath(r.JsonPath)
                                                 })
                                .ToList();
                        }
                    }
                }
            }
        }
    }
}