using System.Diagnostics;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath
{
    [DebuggerDisplay("JsonPath: {Path}")]
    public class JsonPath : IJsonPath
    {
        private string[] _parts;

        public JsonPath(string path)
        {
            Path = path;
        }

        #region IJsonPath Members

        public string Path { get; private set; }

        public IJsonValue GetValue(IJsonValue obj)
        {
            _parts = (_parts ?? Path.Split('.'));
            foreach (var part in _parts)
            {
                var o = obj as IJsonObject;
                if (o == null)
                {
                    return null;
                }
                IJsonValue child;
                if (!o.TryGetValue(part, out child))
                {
                    return null;
                }
                obj = child;
            }
            return obj;
        }

        #endregion
    }
}