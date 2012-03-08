using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class BoolComparerVisitor : AbstractComparerVisitor
    {
        private readonly bool _value;

        public BoolComparerVisitor(bool value)
        {
            _value = value;
        }

        public override bool VisitBool(IJsonBool value)
        {
            return _value == value.Value;
        }
    }
}