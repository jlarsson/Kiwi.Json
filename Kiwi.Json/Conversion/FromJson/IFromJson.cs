using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.FromJson
{
    public interface IFromJson
    {
        object FromJson(Type nativeType, IJsonValue value);
    }
}