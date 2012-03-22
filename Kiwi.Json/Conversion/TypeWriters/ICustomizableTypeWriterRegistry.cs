namespace Kiwi.Json.Conversion.TypeWriters
{
    public interface ICustomizableTypeWriterRegistry : ITypeWriterRegistry
    {
        void RegisterConverters(IJsonConverter[] converters);
    }
}