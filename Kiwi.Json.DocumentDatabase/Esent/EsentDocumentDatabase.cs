using System.Collections.Generic;
using System.Linq;
using Kiwi.Fluesent;

namespace Kiwi.Json.DocumentDatabase.Esent
{
    public class EsentDocumentDatabase : IDocumentDatabase
    {
        public EsentDocumentDatabase(IEsentDatabase database)
        {
            Database = database;
            Database.SetCreateDatabaseOptions(Mappings.DatabaseDefinition, false);
        }

        public EsentDocumentDatabase(string path) : this(new EsentDatabase(path))
        {
        }

        public IEsentDatabase Database { get; protected set; }

        #region IDocumentDatabase Members

        public IEnumerable<IDocumentCollection> Collections
        {
            get
            {
                using (var session = Database.CreateSession(true))
                {
                    using (var transaction = session.CreateTransaction())
                    {
                        using (var table = transaction.OpenTable("Collection"))
                        {
                            var cursor = table.CreateCursor(null);

                            return cursor.Scan(Mappings.CollectionRecordMapper)
                                .Select(c => GetCollection(c.CollectionName))
                                .ToList();
                        }
                    }
                }
            }
        }

        public IDocumentCollection GetCollection(string name)
        {
            return new EsentDocumentCollection(Database, name);
        }

        #endregion

        public EsentDocumentDatabase AlwaysCreateNew()
        {
            Database.SetCreateDatabaseOptions(Mappings.DatabaseDefinition, true);
            return this;
        }

        public static void Collect()
        {
            InstanceCache.Collect();
        }
    }
}