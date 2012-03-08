using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public interface IJsonFilterMatcher
    {
        IEnumerable<IJsonValue> GetFilterValues(IJsonValue value);
        bool IsFilterMatch(IJsonValue filter, IJsonValue value);
    }
}