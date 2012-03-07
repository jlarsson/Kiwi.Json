using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath
{
    public interface IJsonPath
    {
        string Path { get; }
        IEnumerable<IJsonValue> Evaluate(IJsonValue obj);
    }
}