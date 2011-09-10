using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ToJsonDouble : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            return context.CreateNumber((double) value);
        }

        #endregion
    }
}