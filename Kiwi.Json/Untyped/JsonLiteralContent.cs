using System.Collections.Generic;
using Kiwi.Json.Conversion;
using Kiwi.Json.JPath;

namespace Kiwi.Json.Untyped
{
    public class JsonLiteralContent : IJsonValue
    {
        private readonly string _content;

        public JsonLiteralContent(string content)
        {
            _content = content;
        }

        #region IJsonValue Members

        public IEnumerable<string> GetJsonPaths(string prefix, bool includeWildcards)
        {
            yield break;
        }

        public IEnumerable<IJsonPathValue> JsonPathValues(string prefix = "$")
        {
            yield break;
        }

        public void Write(IJsonWriter writer)
        {
            writer.WriteLiteralContent(_content);
        }

        public object ToObject()
        {
            return _content;
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return default(T);
        }

        #endregion
    }
}