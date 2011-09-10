using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ToJsonClass : IToJson
    {
        public IMemberGetter[] Getters { get; set; }

        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            var obj = context.CreateObject();
            foreach (var getter in Getters)
            {
                obj.Add(getter.MemberName, context.Convert(getter.GetMemberValue(value)));
            }
            return obj;
        }

        #endregion
    }
}