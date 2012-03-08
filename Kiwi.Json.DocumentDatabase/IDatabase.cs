using System.Collections.Generic;

namespace Kiwi.Json.DocumentDatabase
{
    public interface IDatabase
    {
        IEnumerable<IDocumentCollection> Collections { get; }
        IDocumentCollection GetCollection(string name);
    }
}