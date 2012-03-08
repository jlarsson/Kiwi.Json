using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public interface IJsonFilter
    {
        bool Matches(IJsonValue value);
    }
}