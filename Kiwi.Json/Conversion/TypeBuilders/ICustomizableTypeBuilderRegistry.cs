namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface ICustomizableTypeBuilderRegistry : ITypeBuilderRegistry
    {
        void RegisterConverters(IJsonConverter[] converters);
    }
}