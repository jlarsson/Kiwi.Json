using System.Data.SQLite;
using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class SqliteDocumentCollection : IDocumentCollection
    {
        public SqliteDocumentCollection(string name, MemoryDatabase database)
        {
            Name = name;
            Database = database;
        }

        public MemoryDatabase Database { get; set; }
        public SQLiteConnection Connection { get; set; }

        #region IDocumentCollection Members

        public string Name { get; set; }

        public ICollectionSession CreateSession()
        {
            return Database.CreateCollectionSession(this);
        }

        #endregion
    }
}