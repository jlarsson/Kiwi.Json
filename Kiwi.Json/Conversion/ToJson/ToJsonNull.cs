using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.ToJson
{
    public class ToJsonNull : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            return context.CreateNull();
        }

        #endregion
    }
}