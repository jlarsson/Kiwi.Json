using System;

namespace Kiwi.Json.Untyped
{
    public interface IJsonFactory
    {
        IJsonValue TryCreatePrimitiveValue(object value);
        IJsonObject CreateObject();
        IJsonArray CreateArray();
        IJsonValue CreateString(string value);
        IJsonValue CreateNumber(int value);
        IJsonValue CreateNumber(double value);
        IJsonValue CreateBool(bool value);
        IJsonValue CreateDate(DateTime value);
        IJsonValue CreateNull();
    }
}