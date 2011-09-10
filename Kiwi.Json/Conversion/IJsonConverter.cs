using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public interface IJsonConverter
    {
        IJsonValue ToJson(object value);
        object FromJson(Type nativeType, IJsonValue value);
    }
}