using System;

namespace Kiwi.Json.Untyped
{
    public interface IJsonFactory
    {
        IJsonObject CreateObject();
        IJsonArray CreateArray();
        IJsonValue CreateString(string value);
        IJsonValue CreateNumber(long value);
        IJsonValue CreateNumber(double value);
        IJsonValue CreateBool(bool value);
        IJsonValue CreateDate(DateTime value);
        IJsonValue CreateNull();
    }
}