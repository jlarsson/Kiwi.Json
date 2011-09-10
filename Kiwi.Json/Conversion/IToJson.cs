using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public interface IToJson
    {
        IJsonValue ToJson(object value, IToJsonContext context);
    }
}