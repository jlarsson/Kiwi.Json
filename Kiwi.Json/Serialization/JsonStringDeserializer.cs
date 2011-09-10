using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization
{
    public class JsonStringDeserializer : AbstractJsonDeserializer
    {
        private readonly string _json;
        private int _index;

        public JsonStringDeserializer(string json)
        {
            _json = json;
        }

        public JsonStringDeserializer(string json, IJsonFactory factory) : base(factory)
        {
            _json = json;
        }

        protected override void SkipWhitespace()
        {
            while ((_index < _json.Length) && (char.IsWhiteSpace(_json[_index])))
            {
                ++_index;
            }
        }

        protected override int Read()
        {
            return _index < _json.Length ? _json[_index++] : -1;
        }

        protected override int Peek()
        {
            return _index < _json.Length ? _json[_index] : -1;
        }
    }
}