using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Evaluators
{
    public class NamedMemberEvaluator : NullJsonValueVisitor<IJsonValue>, IJsonPathPartEvaluator
    {
        public NamedMemberEvaluator(string memberName)
        {
            MemberName = memberName;
        }

        public string MemberName { get; private set; }

        #region IJsonPathPartEvaluator Members

        public JsonPathFlags Flags
        {
            get { return JsonPathFlags.None; }
        }

        public IEnumerable<IJsonValue> Evaluate(IEnumerable<IJsonValue> values)
        {
            return from value in values let v = value.Visit(this) where v != null select v;
        }

        #endregion

        public override IJsonValue VisitObject(IJsonObject value)
        {
            IJsonValue member;
            return value.TryGetValue(MemberName, out member) ? member : null;
        }
    }
}