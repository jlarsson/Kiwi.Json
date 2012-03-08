using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class ObjectComparerVisitor : AbstractComparerVisitor
    {
        private readonly IJsonObject _value;

        public ObjectComparerVisitor(IJsonObject value)
        {
            _value = value;
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

                var filter = new JsonFilter(kv.Value);
                if (!filter.Matches(v))
                {
                    return false;
                }
            }
            return true;
        }
    }
}