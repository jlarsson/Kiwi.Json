using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ToJsonSingle : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            return context.CreateNumber((Single) value);
        }

        #endregion
    }
}