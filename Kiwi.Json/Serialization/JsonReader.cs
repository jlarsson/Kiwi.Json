using System.IO;

namespace Kiwi.Json.Serialization
{
    public class JsonReader : AbstractJsonReader
    {
        private readonly TextReader _reader;

        public JsonReader(TextReader reader)
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