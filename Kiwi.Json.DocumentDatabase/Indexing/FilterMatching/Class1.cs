using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class ArrayComparerVisitor : AbstractComparerVisitor
    {
        private readonly IJsonFilterMatcher _matcher;
        private readonly IJsonArray _value;

        public ArrayComparerVisitor(IJsonArray value, IJsonFilterMatcher matcher)
        {
            _value = value;
            _matcher = matcher;
        }

        public override bool VisitArray(IJsonArray value)
        {
            return
                (_value.Count == 0)
                ||
                _value.All(a => value.Any(aa => _matcher.IsFilterMatch(a, aa)));
        }
    }
}