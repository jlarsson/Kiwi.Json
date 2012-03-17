using System.IO;

namespace Kiwi.Json.Conversion
{
    public class JsonTextParser : AbstractJsonParser
    {
        private readonly TextReader _reader;

        public JsonTextParser(TextReader reader)
        {
            _reader = reader;
        }

        protected override int Read()
        {
            return _reader.Read();
        }

        protected override int Peek()
        {
            return _reader.Peek();
        }
    }
}