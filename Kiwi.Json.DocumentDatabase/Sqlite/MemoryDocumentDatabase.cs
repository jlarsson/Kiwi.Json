using System.Data.SQLite;
using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class MemoryDocumentDatabase : AbstractDocumentDatabase
    {
        public MemoryDocumentDatabase()
        {
            Connection = new SQLiteConnection(@"Data Source=:memory:");
            Connection.Open();

            EnforceSchema();
        }

        protected SQLiteConnection Connection { get; private set; }
        protected override IDbSession CreateSession()
        {
            return new DbSession(Connection, false);
        }
    }
}