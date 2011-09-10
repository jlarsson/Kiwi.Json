using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath
{
    public interface IJsonPathValue
    {
        IJsonPath Path { get; }
        IJsonValue Value { get; }
    }
}