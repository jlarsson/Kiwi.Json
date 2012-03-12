using System.Collections.Generic;
using System.Linq;
using Kiwi.Fluesent;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Json.DocumentDatabase.Esent
{
    public class EsentDocumentDatabase: IDocumentDatabase
    {
        public IEsentDatabase Database { get; protected set; }

        public EsentDocumentDatabase(IEsentDatabase database)
        {
            Database = database;
        }

        public EsentDocumentDatabase(string path): this(new EsentDatabase(path))
        {
        }

        public IEnumerable<IDocumentCollection> Collections
        {
            get
            {
                using (var instance = Database.CreateInstance())
                {
                    using (var session = instance.CreateSession())
                    {
                        using (var transaction = session.CreateTransaction())
                        {
                            using (var table = transaction.OpenTable("Collection"))
                            {
                                var search = table.CreateSearch(Mappings.CollectionRecordMapper);

                                return search.FindAll().Select(r => GetCollection(r.CollectionName)).ToList();
                            }
                        }
                    }
                }
            }
        }

        public IDocumentCollection GetCollection(string name)
        {
            return new EsentDocumentCollection(Database, name);
        }

        public void CreateDatabase()
        {
            using (var instance = Database.CreateInstance())
            {
                using (var session = instance.CreateSession(false))
                {
                    session.CreateDatabase(null, CreateDatabaseGrbit.OverwriteExisting);
                    using (var transaction = session.CreateTransaction())
                    {
                        Mappings.DatabaseDefinition.Create(transaction);

                        transaction.Commit();
                    }
                }
            }
        }
    }
}
