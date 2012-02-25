namespace Kiwi.Json.Serialization.Serializers
{
    public interface ITypeWriter
    {
        void Serialize(ITypeWriterRegistry registry, IJsonWriter writer, object value);
    }
}