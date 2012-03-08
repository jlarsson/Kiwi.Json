using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public interface IConvertJsonValueToIndexNormalForm
    {
        IJsonValue Convert(IJsonValue value);
    }
}