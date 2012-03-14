using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Evaluators
{
    public class IndexedElementEvaluator : NullJsonValueVisitor<IJsonValue>, IJsonPathPartEvaluator
    {
        public IndexedElementEvaluator(int index)
        {
            Index = index;
        }

        protected int Index { get; private set; }

        #region IJsonPathPartEvaluator Members

        public JsonPathFlags Flags
        {
            get { return JsonPathFlags.HasArrayIndex; }
        }

        public IEnumerable<IJsonValue> Evaluate(IEnumerable<IJsonValue> values)
        {
            return from value in values let v = value.Visit(this) where v != null select v;
        }

        #endregion

        public override IJsonValue VisitArray(IJsonArray value)
        {
            if (value.Count > Index)
            {
                return value[Index];
            }
            return null;
        }
    }
}