using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface ITypeBuilderRegistry
    {
        ITypeBuilder GetTypeBuilder<T>();
        ITypeBuilder GetTypeBuilder(Type type);
    }
}