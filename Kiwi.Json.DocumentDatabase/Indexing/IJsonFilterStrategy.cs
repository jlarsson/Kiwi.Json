using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public interface IJsonFilterStrategy
    {
        IJsonFilter CreateFilter(IJsonValue filter);
        IEnumerable<IJsonValue> GetFilterValues(IJsonValue value);
    }
}