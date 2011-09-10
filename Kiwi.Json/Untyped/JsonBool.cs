using System.Collections.Generic;
using System.Diagnostics;
using Kiwi.Json.JPath;
using Kiwi.Json.Serialization;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonBool : IJsonBool
    {
        public JsonBool(bool value)
        {
            Value = value;
        }

        #region IJsonBool Members

        public bool Value { get; private set; }

        public IEnumerable<IJsonPathValue> JsonPathValues(string prefix)
        {
            yield return new JsonPathValue(prefix, this);
        }

        public void Write(IJsonWriter writer)
        {
            writer.WriteBool(Value);
        }

        public object ToObject()
        {
            return Value;
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return visitor.VisitBool(this);
        }

        #endregion

        public override string ToString()
        {
            return this.PrettyPrint();
        }
    }
}