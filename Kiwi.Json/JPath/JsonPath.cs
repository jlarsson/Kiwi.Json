using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath
{
    [DebuggerDisplay("JsonPath: {Path}")]
    public class JsonPath : IJsonPath
    {
        private readonly IJsonPathPartEvaluator[] _evaluators;

        public JsonPath(string path)
        {
            Path = path;
            _evaluators = new JsonPathParserRunner(path).Parse();
        }

        #region IJsonPath Members

        public string Path { get; private set; }

        public IEnumerable<IJsonValue> Evaluate(IJsonValue obj)
        {
            if (obj == null)
            {
                return null;
            }
            IEnumerable<IJsonValue> values = new[] {obj};
            return _evaluators.Aggregate(values,
                                         (current, evaluator) => evaluator.Evaluate(current).Where(v => v != null));
        }

        #endregion
    }
}