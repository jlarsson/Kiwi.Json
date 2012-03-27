using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public static class TypeBuilderRegistryExtensions
    {
        public static ITypeBuilder GetTypeBuilder<T>(this ITypeBuilderRegistry registry)
        {
            return registry.GetTypeBuilder(typeof (T));
        }

        public static object Read(this ITypeBuilderRegistry registry, Type type, IJsonParser parser, object instanceState)
        {
            return parser.Parse(registry, registry.GetTypeBuilder(type), instanceState);
        }

        public static T Read<T>(this ITypeBuilderRegistry registry, IJsonParser parser, object instanceState)
        {
            return (T) parser.Parse(registry, registry.GetTypeBuilder<T>(), instanceState);
        }
    }
}