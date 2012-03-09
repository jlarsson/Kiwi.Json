using System.Collections.Generic;

namespace Kiwi.Json.DocumentDatabase
{
    public interface IDocumentDatabase
    {
        IEnumerable<IDocumentCollection> Collections { get; }
        IDocumentCollection GetCollection(string name);
    }
}