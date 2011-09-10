using System;

namespace Kiwi.Json.Conversion
{
    public interface IJsonConverterFactory
    {
        IToJson GetNativeToJsonConverter(Type type);
    }
}