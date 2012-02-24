using System.Collections.Generic;
using Kiwi.Json.JPath;
using Kiwi.Json.Serialization;

namespace Kiwi.Json.Untyped
{
    public interface IJsonValue
    {
        IEnumerable<IJsonPathValue> JsonPathValues(string prefix = "");
        void Write(IJsonWriter writer);
        object ToObject();
        T Visit<T>(IJsonValueVisitor<T> visitor);
    }
}