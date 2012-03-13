using System.Collections.Generic;
using Kiwi.Json.JPath;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase
{
    public interface IDocumentCollectionIndex
    {
        IJsonPath JsonPath { get; }
        IEnumerable<IJsonValue> GetValues(string key);
        IEnumerable<KeyValuePair<string, IJsonValue>> GetValues();
    }
}