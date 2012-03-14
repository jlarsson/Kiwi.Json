using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Evaluators
{
    public class ArraySliceEvaluator : IJsonPathPartEvaluator, IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        private readonly int? _end;
        private readonly int? _start;
        private readonly int? _step;

        public ArraySliceEvaluator(int? start, int? end, int? step)
        {
            _start = start;
            _end = end;
            _step = step;
        }

        #region IJsonPathPartEvaluator Members

        public JsonPathFlags Flags
        {
            get { return JsonPathFlags.HasArrayIndex | JsonPathFlags.HasArraySlice; }
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
            var start = _start.HasValue ? _start.Value : 0;
            var end = _end.HasValue ? _end.Value : value.Count;
            var step = _step.HasValue ? _step.Value : 1;

            if (step == 0)
            {
                yield break;
            }

            if (start < 0)
            {
                start = value.Count + start;
            }
            if (end < 0)
            {
                end = value.Count + end;
            }
            var index = start;

            if (start < end)
            {
                while ((index >= 0) && (index < value.Count) && (index <= end))
                {
                    yield return value[index];
                    index += step;
                }
            }
            else
            {
                while ((index >= 0) && (index < value.Count) && (index >= end))
                {
                    yield return value[index];
                    index += step;
                }
            }
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