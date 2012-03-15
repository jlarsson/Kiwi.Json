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
        public JsonObject()
        {
        }

        public JsonObject(IDictionary<string, IJsonValue> dictionary)
            : base(dictionary)
        {
        }

        #region IJsonObject Members

        public IEnumerable<string> GetJsonPaths(string prefix, bool includeWildcards)
        {
            string head = null;
            IEnumerable<string> tail = null;
            if (string.IsNullOrEmpty(prefix))
            {
                tail = this.Select(kv => kv.Value.GetJsonPaths(kv.Key, includeWildcards)).SelectMany(l => l);
            }
            else
            {
                if (includeWildcards)
                {
                    head = prefix + ".*";
                }
                tail = this.Select(kv => kv.Value.GetJsonPaths(prefix + '.' + kv.Key, includeWildcards)).SelectMany(l => l);
            }
            if (head != null)
            {
                yield return head;
            }
            yield return prefix;
            foreach (var t in tail)
            {
                yield return t;
            }
        }

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
            return JsonConvert.Write(this);
        }
    }
}