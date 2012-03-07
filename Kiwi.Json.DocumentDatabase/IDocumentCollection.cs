using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase
{
    public interface IDocumentCollection
    {
        string Name { get; }
        ICollectionSession CreateSession();
    }
}