using System.Collections.Generic;

namespace Kiwi.Json.Untyped
{
    public interface IJsonObject : IDictionary<string,IJsonValue>, IJsonValue
    {
    }
}