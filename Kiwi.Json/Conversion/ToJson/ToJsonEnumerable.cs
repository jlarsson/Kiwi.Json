using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.ToJson
{
    public class ToJsonEnumerable<T> : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            var array = context.CreateArray();
            foreach (var item in (IEnumerable<T>) value)
            {
                array.Add(context.Convert(item));
            }
            return array;
        }

        #endregion
    }
}