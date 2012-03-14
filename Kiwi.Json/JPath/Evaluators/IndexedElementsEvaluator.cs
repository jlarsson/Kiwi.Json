using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Evaluators
{
    public class IndexedElementsEvaluator : IJsonPathPartEvaluator, IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        private readonly int[] _indexes;

        public IndexedElementsEvaluator(int[] indexes)
        {
            _indexes = indexes;
        }

        #region IJsonPathPartEvaluator Members

        public JsonPathFlags Flags
        {
            get { return JsonPathFlags.HasArrayIndex; }
        }

        public IEnumerable<IJsonValue> Evaluate(IEnumerable<IJsonValue> values)
        {
            return from value in values
                   from v in value.Visit(this)
                   select v;
        }

        #endregion

        #region IJsonValueVisitor<IEnumerable<IJsonValue>> Members

        public IEnumerable<IJsonValue> VisitArray(IJsonArray value)
        {
            return _indexes.Distinct().Where(i => (i >= 0) && (i < value.Count)).Select(i => value[i]);
        }

        public IEnumerable<IJsonValue> VisitBool(IJsonBool value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitDate(IJsonDate value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitDouble(IJsonDouble value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitInteger(IJsonInteger value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitNull(IJsonNull value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitObject(IJsonObject value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitString(IJsonString value)
        {
            yield break;
        }

        #endregion
    }
}