using System.Collections.Generic;
using System.Diagnostics;
using Kiwi.Json.JPath;
using Kiwi.Json.Serialization;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonInteger : IJsonInteger
    {
        public JsonInteger(int value)
        {
            Value = value;
        }

        #region IJsonInteger Members

        public int Value { get; private set; }

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
            return this.PrettyPrint();
        }
    }
}