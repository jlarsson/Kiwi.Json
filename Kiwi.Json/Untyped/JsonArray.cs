using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kiwi.Json.Conversion;
using Kiwi.Json.JPath;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonArray : List<IJsonValue>, IJsonArray
    {
        public JsonArray()
        {
        }

        public JsonArray(IEnumerable<IJsonValue> collection) : base(collection)
        {
        }

        #region IJsonArray Members

        public IEnumerable<string> GetJsonPaths(string prefix, bool includeWildcards)
        {
            yield return prefix;

            if (includeWildcards)
            {
                var wc = prefix + "[*]";
                yield return wc;
                foreach (var p in from element in this
                                  from p in element.GetJsonPaths(wc, includeWildcards)
                                  select p)
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<IJsonPathValue> JsonPathValues(string prefix)
        {
            yield return new JsonPathValue(prefix, this);
        }

        public void Write(IJsonWriter writer)
        {
            writer.WriteArrayStart();

            var index = 0;
            foreach (var item in this)
            {
                if (index++ > 0)
                {
                    writer.WriteArrayElementDelimiter();
                }
                item.Write(writer);
            }
            writer.WriteArrayEnd(index);
        }

        public object ToObject()
        {
            return this.Select(elem => elem.ToObject()).ToList();
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return visitor.VisitArray(this);
        }

        #endregion

        public override string ToString()
        {
            return JSON.Write(this);
        }
    }
}