using System.Collections.Generic;
using System.Diagnostics;
using Kiwi.Json.JPath;
using Kiwi.Json.Serialization;

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
            return this.PrettyPrint();
        }
    }
}