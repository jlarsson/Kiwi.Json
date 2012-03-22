namespace Kiwi.Json.Conversion.TypeWriters
{
    public static class TypeWriterRegistryExtensions
    {
        public static void Write(this ITypeWriterRegistry registry, IJsonWriter writer, object obj)
        {
            var w = registry.GetTypeWriterForValue(obj);
            registry.GetTypeWriterForValue(obj).Write(writer, registry, obj);
        }
    }
}