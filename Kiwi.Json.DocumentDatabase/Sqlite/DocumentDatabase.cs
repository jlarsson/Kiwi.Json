using System.Data.Common;
using System.Data.SQLite;
using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class DocumentDatabase: AbstractDocumentDatabase
    {
        private readonly string _filePath;

        public DocumentDatabase(string filePath)
        {
            _filePath = filePath;
            EnforceSchema();
        }

        protected override IDbSession CreateSession()
        {
            return new DbSession(new SQLiteConnection(@"Data Source="+_filePath));
        }
    }
}