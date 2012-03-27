using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface ITypeBuilderRegistry
    {
        ITypeBuilder GetTypeBuilder(Type type);
    }
}