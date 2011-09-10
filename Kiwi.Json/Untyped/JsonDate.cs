using System;
using System.Collections.Generic;
using System.Diagnostics;
using Kiwi.Json.JPath;
using Kiwi.Json.Serialization;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonDate : IJsonDate
    {
        public JsonDate(DateTime value)
        {
            Value = value;
        }

        #region IJsonDate Members

        public DateTime Value { get; private set; }

        public IEnumerable<IJsonPathValue> JsonPathValues(string prefix)
        {
            yield return new JsonPathValue(prefix, this);
        }

        public void Write(IJsonWriter writer)
        {
            writer.WriteDate(Value);
        }

        public object ToObject()
        {
            return Value;
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return visitor.VisitDate(this);
        }

        #endregion

        public override string ToString()
        {
            return this.PrettyPrint();
        }
    }
}