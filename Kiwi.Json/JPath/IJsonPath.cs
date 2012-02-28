using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath
{
    public interface IJsonPath
    {
        string Path { get; }
        IJsonValue GetValue(IJsonValue obj);
    }
}