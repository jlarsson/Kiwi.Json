using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public interface IToJsonContext : IJsonFactory
    {
        IJsonValue Convert(object value);
    }
}