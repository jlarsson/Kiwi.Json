using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath
{
    public interface IJsonPathPartEvaluator
    {
        JsonPathFlags Flags { get; }
        IEnumerable<IJsonValue> Evaluate(IEnumerable<IJsonValue> values);
    }
}