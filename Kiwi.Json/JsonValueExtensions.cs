using Kiwi.Json.Serialization;
using Kiwi.Json.Untyped;

namespace Kiwi.Json
{
    public static class JsonValueExtensions
    {
        public static string PrettyPrint(this IJsonValue value)
        {
            var writer = new JsonStringWriter();
            value.Write(writer);
            return writer.ToString();
        }
        
        public static T ConvertTo<T>(this IJsonValue value)
        {
            return JSON.ToObject<T>(value);
        }
    }
}