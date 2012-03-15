using System.Collections.Generic;
using System.Diagnostics;
using Kiwi.Json.JPath;
using Kiwi.Json.Conversion;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonInteger : IJsonInteger
    {
        public JsonInteger(long value)
        {
            Value = value;
        }

        #region IJsonInteger Members

        public long Value { get; private set; }

        public IEnumerable<string> GetJsonPaths(string prefix, bool includeWildcards)
        {
            yield return prefix;
        }

        public IEnumerable<IJsonPathValue> JsonPathValues(string prefix)
        {
            yield return new JsonPathValue(prefix, this);
        }

        public void Write(IJsonWriter writer)
        {
            writer.WriteInteger(Value);
        }

        public object ToObject()
        {
            return Value;
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return visitor.VisitInteger(this);
        }

        #endregion

        public override string ToString()
        {
            return JsonConvert.Write(this);
        }
    }
}