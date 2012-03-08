using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class ObjectComparerVisitor : AbstractComparerVisitor
    {
        private readonly IJsonObject _value;
        private readonly IJsonFilterMatcher _matcher;

        public ObjectComparerVisitor(IJsonObject value, IJsonFilterMatcher matcher)
        {
            _value = value;
            _matcher = matcher;
        }

        public override bool VisitObject(IJsonObject value)
        {
            foreach (var kv in _value)
            {
                IJsonValue v;
                if (!value.TryGetValue(kv.Key, out v))
                {
                    return false;
                }
                if (!_matcher.IsFilterMatch(kv.Value, v))
                {
                    return false;
                }
            }
            return true;
        }
    }
}