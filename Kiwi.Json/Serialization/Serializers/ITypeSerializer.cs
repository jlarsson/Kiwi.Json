namespace Kiwi.Json.Serialization.Serializers
{
    public interface ITypeSerializer
    {
        void Serialize(ITypeSerializerRegistry registry, IJsonWriter writer, object value);
    }
}