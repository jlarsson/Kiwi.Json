using System;

namespace Kiwi.Json.Serialization
{
    public interface ITypeBuilder
    {
        IObjectBuilder CreateObject();
        IArrayBuilder CreateArray();

        object CreateString(string value);
        object CreateNumber(long value);
        object CreateNumber(double value);
        object CreateBool(bool value);
        object CreateDateTime(DateTime value);
        object CreateNull();
    }
}