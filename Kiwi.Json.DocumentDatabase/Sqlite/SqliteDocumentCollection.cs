using System.Collections.Generic;
using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class SqliteDocumentCollection : IDocumentCollection
    {
        public SqliteDocumentCollection(string name, AbstractDocumentDatabase documentDatabase)
        {
            Name = name;
            DocumentDatabase = documentDatabase;
        }

        public AbstractDocumentDatabase DocumentDatabase { get; set; }

        #region IDocumentCollection Members

        public string Name { get; set; }

        public ICollectionSession CreateSession()
        {
            return DocumentDatabase.CreateCollectionSession(this);
        }

        public IEnumerable<IDocumentCollectionIndex> GetIndexes()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}