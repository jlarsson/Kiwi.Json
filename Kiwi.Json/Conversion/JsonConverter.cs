using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class JsonConverter : IJsonConverter
    {
        public JsonConverter()
        {
            JsonFactory = new JsonFactory();
            ToJsonConverter = new DefaultToJson();
            FromJsonConverter = new DefaultFromJson();
        }

        public IJsonFactory JsonFactory { get; set; }
        public IToJson ToJsonConverter { get; set; }
        public IFromJson FromJsonConverter { get; set; }

        #region IJsonConverter Members

        public IJsonValue ToJson(object value)
        {
            return ToJsonConverter.ToJson(value, new ToJsonContext(JsonFactory, ToJsonConverter));
        }

        public object FromJson(Type nativeType, IJsonValue value)
        {
            return FromJsonConverter.FromJson(nativeType, value);
        }

        #endregion
    }
}