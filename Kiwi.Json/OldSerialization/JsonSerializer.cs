using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization
{
    public class JsonSerializer
    {
        public void Serialize(IJsonValue value, IJsonWriter writer)
        {
            value.Write(writer);
        }
    }
}