using System.Collections.Generic;
using System.Diagnostics;
using Kiwi.Json.Conversion;
using Kiwi.Json.JPath;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonDouble : IJsonDouble
    {
        public JsonDouble(double value)
        {
            Value = value;
        }

        #region IJsonDouble Members

        public double Value { get; private set; }

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
            writer.WriteDouble(Value);
        }

        public object ToObject()
        {
            return Value;
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return visitor.VisitDouble(this);
        }

        #endregion

        public override string ToString()
        {
            return JsonConvert.Write(this);
        }
    }
}