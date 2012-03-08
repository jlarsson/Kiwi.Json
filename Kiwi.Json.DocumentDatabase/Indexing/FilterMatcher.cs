using System.Collections.Generic;
using Kiwi.Json.DocumentDatabase.Indexing.FilterMatching;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public class FilterMatcher: IJsonFilterMatcher
    {
        public IEnumerable<IJsonValue> GetFilterValues(IJsonValue value)
        {
            return value.Visit(new ExtractFilterValuesVisitor());
        }

        public bool IsFilterMatch(IJsonValue filter, IJsonValue value)
        {
            return IsFilterMatchRecursive(filter, value);
        }

        private bool IsFilterMatchRecursive(IJsonValue filter, IJsonValue value)
        {
            if ((filter == null) || (value == null))
            {
                return filter == value;
            }

            var comparer = filter.Visit(new CreateFilterComparerVisitor(this));
            return value.Visit(comparer);
        }
    }
}