using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class NullComparerVisitor : AbstractComparerVisitor
    {
        public override bool VisitNull(IJsonNull value)
        {
            return true;
        }
    }
}