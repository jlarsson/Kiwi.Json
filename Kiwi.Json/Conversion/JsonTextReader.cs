using System.IO;

namespace Kiwi.Json.Conversion
{
    public class JsonTextReader : AbstractJsonReader
    {
        private readonly TextReader _reader;

        public JsonTextReader(TextReader reader)
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