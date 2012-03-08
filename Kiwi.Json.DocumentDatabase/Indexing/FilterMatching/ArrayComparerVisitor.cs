using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class ArrayComparerVisitor : AbstractComparerVisitor
    {
        private readonly IJsonArray _value;

        public ArrayComparerVisitor(IJsonArray value)
        {
            _value = value;
        }

        public override bool VisitArray(IJsonArray value)
        {
            if (_value.Count == 0)
            {
                return true;
            }
            return _value.Select(elem => new JsonFilter(elem)).All(filter => value.Any(filter.Matches));
        }
    }
}