using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kiwi.Json.JPath;
using Kiwi.Json.Conversion;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonObject : Dictionary<string, IJsonValue>, IJsonObject
    {
        #region IJsonObject Members

        public IEnumerable<IJsonPathValue> JsonPathValues(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return this.Select(kv => kv.Value.JsonPathValues(kv.Key)).SelectMany(l => l);
            }
            return this.Select(kv => kv.Value.JsonPathValues(prefix + "." + kv.Key)).SelectMany(l => l);
        }

        public void Write(IJsonWriter writer)
        {
            writer.WriteObjectStart();
            var index = 0;
            foreach (var kv in this)
            {
                if (index++ > 0)
                {
                    writer.WriteObjectMemberDelimiter();
                }
                writer.WriteMember(kv.Key);
                kv.Value.Write(writer);
            }
            writer.WriteObjectEnd(index);
        }

        public object ToObject()
        {
            return this.ToDictionary(kv => kv.Key, kv => kv.Value.ToObject());
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return visitor.VisitObject(this);
        }

        #endregion

        public override string ToString()
        {
            return this.PrettyPrint();
        }
    }
}