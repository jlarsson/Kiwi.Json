namespace Kiwi.Json
{
    public class InvalidClassForDeserializationException : JsonException
    {
        public InvalidClassForDeserializationException(string message) : base(message)
        {
        }
    }
}