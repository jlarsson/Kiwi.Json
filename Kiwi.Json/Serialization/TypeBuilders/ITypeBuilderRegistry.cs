using System;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public interface ITypeBuilderRegistry
    {
        ITypeBuilder GetTypeBuilder<T>();
        ITypeBuilder GetTypeBuilder(Type type);
    }
}