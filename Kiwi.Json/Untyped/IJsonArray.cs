using System.Collections.Generic;

namespace Kiwi.Json.Untyped
{
    public interface IJsonArray : IList<IJsonValue>, IJsonValue
    {
        int Capacity { get; set; }
    }
}