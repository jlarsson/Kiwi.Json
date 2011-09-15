using System.IO;
using Kiwi.Json.Serialization;
using Kiwi.Json.Untyped;

namespace Kiwi.Json
{
    public static class JsonValueExtensions
    {
        public static void Write(this IJsonValue value, TextWriter writer)
        {
            value.Write(new JsonTextWriter(writer));
        }

        public static string PrettyPrint(this IJsonValue value)
        {
            var writer = new StringWriter();
            //value.Write(new JsonTextWriter(writer, new JsonTextIndent("\t")));
            value.Write(new JsonTextWriter(writer, JsonTextIndent.NoIndent));
            return writer.ToString();
        }

        public static T ConvertTo<T>(this IJsonValue value)
        {
            return JSON.ToObject<T>(value);
        }
    }
}