namespace Kiwi.Json.Serialization.Serializers
{
    public interface ITypeWriter
    {
        void Serialize(IJsonWriter writer, object value);
    }
}