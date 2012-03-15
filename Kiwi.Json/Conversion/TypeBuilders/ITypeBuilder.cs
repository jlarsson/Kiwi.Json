using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface ITypeBuilder
    {
        IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry);
        IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry);

        object CreateString(ITypeBuilderRegistry registry, string value);
        object CreateNumber(ITypeBuilderRegistry registry, long value);
        object CreateNumber(ITypeBuilderRegistry registry, double value);
        object CreateBool(ITypeBuilderRegistry registry, bool value);
        object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue);
        object CreateNull(ITypeBuilderRegistry registry);
    }
}