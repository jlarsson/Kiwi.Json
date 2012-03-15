using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface ITypeBuilderFactory
    {
        Func<ITypeBuilder> CreateTypeBuilder(Type type);
    }
}