using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.ToJson
{
    public interface IToJson
    {
        IJsonValue ToJson(object value, IToJsonContext context);
    }
}