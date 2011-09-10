using System.IO;

namespace Kiwi.Json.Serialization
{
    public class JsonDeserializer : AbstractJsonDeserializer
    {
        private readonly TextReader _reader;

        public JsonDeserializer(TextReader reader)
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