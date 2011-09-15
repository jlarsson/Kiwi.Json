using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.ToJson
{
    public class ToJsonDateTime : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            return context.CreateDate((DateTime) value);
        }

        #endregion
    }
}