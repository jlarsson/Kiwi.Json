using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ToJsonContext : IToJsonContext
    {
        public ToJsonContext(IJsonFactory jsonFactory, IToJson toJson)
        {
            JsonFactory = jsonFactory;
            ToJson = toJson;
        }

        public IJsonFactory JsonFactory { get; private set; }
        public IToJson ToJson { get; private set; }

        #region IToJsonContext Members

        public IJsonValue TryCreatePrimitiveValue(object value)
        {
            return JsonFactory.TryCreatePrimitiveValue(value);
        }

        public IJsonObject CreateObject()
        {
            return JsonFactory.CreateObject();
        }

        public IJsonArray CreateArray()
        {
            return JsonFactory.CreateArray();
        }

        public IJsonValue CreateString(string value)
        {
            return JsonFactory.CreateString(value);
        }

        public IJsonValue CreateNumber(int value)
        {
            return JsonFactory.CreateNumber(value);
        }

        public IJsonValue CreateNumber(double value)
        {
            return JsonFactory.CreateNumber(value);
        }

        public IJsonValue CreateBool(bool value)
        {
            return JsonFactory.CreateBool(value);
        }

        public IJsonValue CreateDate(DateTime value)
        {
            return JsonFactory.CreateDate(value);
        }

        public IJsonValue CreateNull()
        {
            return JsonFactory.CreateNull();
        }

        public IJsonValue Convert(object value)
        {
            return ToJson.ToJson(value, this);
        }

        #endregion
    }
}