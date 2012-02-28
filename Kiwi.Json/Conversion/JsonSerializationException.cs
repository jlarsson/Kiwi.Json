namespace Kiwi.Json.Conversion
{
    public class JsonSerializationException : JsonException
    {
        public JsonSerializationException(string message)
            : base(message)
        {
        }
    }
}