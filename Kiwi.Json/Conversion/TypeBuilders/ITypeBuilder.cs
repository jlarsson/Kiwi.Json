using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface ITypeBuilder
    {
        IObjectBuilder CreateObjectBuilder();
        IArrayBuilder CreateArrayBuilder();

        object CreateString(string value);
        object CreateNumber(long value);
        object CreateNumber(double value);
        object CreateBool(bool value);
        object CreateDateTime(DateTime value, object sourceValue);
        object CreateNull();
    }
}