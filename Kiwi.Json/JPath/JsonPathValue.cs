using System.Diagnostics;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath
{
    [DebuggerDisplay("{Path} -> {Value}")]
    public class JsonPathValue : IJsonPathValue
    {
        public JsonPathValue(string path, IJsonValue value)
        {
            Path = new JsonPath(path);
            Value = value;
        }

        #region IJsonPathValue Members

        public IJsonPath Path { get; set; }
        public IJsonValue Value { get; set; }

        #endregion
    }
}