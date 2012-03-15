using System.Collections.Generic;
using System.Diagnostics;
using Kiwi.Json.JPath;
using Kiwi.Json.Conversion;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonNull : IJsonNull
    {
        #region IJsonNull Members

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
            writer.WriteNull();
        }

        public object ToObject()
        {
            return null;
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return visitor.VisitNull(this);
        }

        #endregion

        public override string ToString()
        {
            return JsonConvert.Write(this);
        }
    }
}