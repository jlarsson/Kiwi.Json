using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Evaluators
{
    public class MatchAllRecursiveEvaluator : IJsonPathPartEvaluator, IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        public static readonly MatchAllRecursiveEvaluator Default = new MatchAllRecursiveEvaluator();

        #region IJsonPathPartEvaluator Members

        public JsonPathFlags Flags
        {
            get { return JsonPathFlags.HasWildCardMember | JsonPathFlags.HasWildcardIndex; }
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
            return from element in value
                   from v in element.Visit(this)
                   select v;
        }

        public IEnumerable<IJsonValue> VisitBool(IJsonBool value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitDate(IJsonDate value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitDouble(IJsonDouble value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitInteger(IJsonInteger value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitNull(IJsonNull value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitObject(IJsonObject value)
        {
            return from member in value.Values
                   from v in member.Visit(this)
                   select v;
        }

        public IEnumerable<IJsonValue> VisitString(IJsonString value)
        {
            yield return value;
        }

        #endregion
    }
}