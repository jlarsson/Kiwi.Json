using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class SqliteDocumentCollection : IDocumentCollection
    {
        public SqliteDocumentCollection(string name, AbstractDatabase database)
        {
            Name = name;
            Database = database;
        }

        public AbstractDatabase Database { get; set; }

        #region IDocumentCollection Members

        public string Name { get; set; }

        public ICollectionSession CreateSession()
        {
            return Database.CreateCollectionSession(this);
        }

        #endregion
    }
}