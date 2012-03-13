using System.Collections.Generic;
using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase
{
    public interface IDocumentCollection
    {
        string Name { get; }
        ICollectionSession CreateSession();
        IEnumerable<IDocumentCollectionIndex> GetIndexes();
    }
}