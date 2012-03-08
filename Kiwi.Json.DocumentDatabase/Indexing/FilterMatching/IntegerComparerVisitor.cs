using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class IntegerComparerVisitor : AbstractComparerVisitor
    {
        private readonly long _value;

        public IntegerComparerVisitor(long value)
        {
            _value = value;
        }

        public override bool VisitInteger(IJsonInteger value)
        {
            return _value == value.Value;
        }
    }
}