using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.ToJson
{
    public class ToJsonDictionary<TKey, TValue> : IToJson
    {
        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            var obj = context.CreateObject();
            foreach (var kv in (Dictionary<TKey, TValue>) value)
            {
                obj.Add(kv.Key.ToString(), context.Convert(kv.Value));
            }
            return obj;
        }

        #endregion
    }
}