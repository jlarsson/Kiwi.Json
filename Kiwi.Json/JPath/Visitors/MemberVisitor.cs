using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Visitors
{
    internal class MemberVisitor : NullJsonValueVisitor<IJsonValue>
    {
        private readonly string _memberName;

        public MemberVisitor(string memberName)
        {
            _memberName = memberName;
        }

        public override IJsonValue VisitObject(IJsonObject value)
        {
            IJsonValue member;
            return value.TryGetValue(_memberName, out member) ? member : null;
        }
    }
}