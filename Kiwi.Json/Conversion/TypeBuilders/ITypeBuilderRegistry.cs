using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface ITypeBuilderRegistry
    {
        ITypeBuilder GetTypeBuilder<T>();
        ITypeBuilder GetTypeBuilder(Type type);
    }

    public static class TypeBuilderRegistryExtensions
    {
        public static T Read<T>(this ITypeBuilderRegistry registry, IJsonReader reader, object instanceState)
        {
            return (T)reader.Parse(registry, registry.GetTypeBuilder<T>(), instanceState);
        }
    }
}