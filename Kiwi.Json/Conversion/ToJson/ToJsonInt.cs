using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.ToJson
{
    public class ToJsonInt : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            return context.CreateNumber(Convert.ToInt64(value));
        }

        #endregion
    }
}