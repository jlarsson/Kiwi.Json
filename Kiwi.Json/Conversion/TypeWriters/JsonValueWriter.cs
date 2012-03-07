using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class JsonValueWriter : ITypeWriter
    {
        public void Write(IJsonWriter writer, object value)
        {
            ((IJsonValue)value).Write(writer);
        }
    }
}