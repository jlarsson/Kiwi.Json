using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.ToJson
{
    public class ToJsonString : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            return value == null ? context.CreateNull() : context.CreateString((string) value);
        }

        #endregion
    }
}