using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Evaluators
{
    public class MatchObjectRecursiveEvaluator : IJsonPathPartEvaluator, IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        public static readonly MatchObjectRecursiveEvaluator Default = new MatchObjectRecursiveEvaluator();

        #region IJsonPathPartEvaluator Members

        public JsonPathFlags Flags
        {
            get { return JsonPathFlags.HasWildCardMember; }
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
            return from elem in value
                   from v in elem.Visit(this)
                   select v;
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
            yield return value;
            foreach (var v in from member in value.Values
                              from v in member.Visit(this)
                              select v)
            {
                yield return v;
            }
        }

        public IEnumerable<IJsonValue> VisitString(IJsonString value)
        {
            yield break;
        }

        #endregion
    }
}