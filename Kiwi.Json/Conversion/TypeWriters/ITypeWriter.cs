namespace Kiwi.Json.Conversion.TypeWriters
{
    public interface ITypeWriter
    {
        void Write(IJsonWriter writer, object value);
    }
}