using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Evaluators
{
    public class MatchObjectMembersOrArrayElementsEvaluator : IJsonPathPartEvaluator,
                                                         IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        public static readonly MatchObjectMembersOrArrayElementsEvaluator Default = new MatchObjectMembersOrArrayElementsEvaluator();

        #region IJsonPathPartEvaluator Members

        public JsonPathFlags Flags
        {
            get { return JsonPathFlags.HasWildCardMember; }
        }

        public IEnumerable<IJsonValue> Evaluate(IEnumerable<IJsonValue> values)
        {
            return from value in values from v in value.Visit(this) select v;
        }

        #endregion

        #region IJsonValueVisitor<IEnumerable<IJsonValue>> Members

        public IEnumerable<IJsonValue> VisitArray(IJsonArray value)
        {
            return value;
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
            return value.Values;
        }

        public IEnumerable<IJsonValue> VisitString(IJsonString value)
        {
            yield break;
        }

        #endregion
    }
}