using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.ToJson
{
    public interface IToJsonContext : IJsonFactory
    {
        IJsonValue Convert(object value);
    }
}