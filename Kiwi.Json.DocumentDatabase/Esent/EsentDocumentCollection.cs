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
            return new EsentCollectionSession(Database, Name);
        }
    }
}