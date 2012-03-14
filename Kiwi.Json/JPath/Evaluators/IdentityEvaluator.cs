using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Evaluators
{
    public class IdentityEvaluator : IJsonPathPartEvaluator
    {
        public static readonly IdentityEvaluator Default = new IdentityEvaluator();

        #region IJsonPathPartEvaluator Members

        public JsonPathFlags Flags
        {
            get { return JsonPathFlags.None; }
        }

        public IEnumerable<IJsonValue> Evaluate(IEnumerable<IJsonValue> values)
        {
            return values;
        }

        #endregion
    }
}