namespace Kiwi.Json.Conversion
{
    public class JsonStringReader : AbstractJsonReader
    {
        private readonly string _json;
        private int _index;

        public JsonStringReader(string json)
        {
            _json = json;
        }

        protected override void SkipWhitespace()
        {
            while ((_index < _json.Length) && (char.IsWhiteSpace(_json[_index])))
            {
                ++_index;
            }
            //while (char.IsWhiteSpace(_json[_index]))
            //{
            //    ++_index;
            //}
        }

        protected override int Read()
        {
            return _index < _json.Length ? _json[_index++] : char.MinValue;
            //return _json[_index++];
        }

        protected override int Peek()
        {
            return _index < _json.Length ? _json[_index] : char.MinValue;
            //return _json[_index];
        }
    }
}