using System.Collections.Generic;
using Kiwi.Json.Conversion;
using Kiwi.Json.JPath;

namespace Kiwi.Json.Untyped
{
    public interface IJsonValue
    {
        IEnumerable<string> GetJsonPaths(string prefix, bool includeWildcards);
        IEnumerable<IJsonPathValue> JsonPathValues(string prefix = "$");
        void Write(IJsonWriter writer);
        object ToObject();
        T Visit<T>(IJsonValueVisitor<T> visitor);
    }
}