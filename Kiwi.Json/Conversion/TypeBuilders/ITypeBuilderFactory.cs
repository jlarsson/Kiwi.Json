using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface ITypeBuilderFactory
    {
        ITypeBuilder CreateTypeBuilder(Type type);
    }
}