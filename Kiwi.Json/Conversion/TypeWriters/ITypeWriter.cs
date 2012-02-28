namespace Kiwi.Json.Conversion.TypeWriters
{
    public interface ITypeWriter
    {
        void Serialize(IJsonWriter writer, object value);
    }
}