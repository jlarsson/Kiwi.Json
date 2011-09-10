using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ToJsonInt : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            return context.CreateNumber(Convert.ToInt32(value));
        }

        #endregion
    }
}