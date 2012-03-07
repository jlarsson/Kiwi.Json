using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Visitors
{
    internal class IndexedMemberVisitor : NullJsonValueVisitor<IJsonValue>
    {
        private readonly int _index;

        public IndexedMemberVisitor(int index)
        {
            _index = index;
        }

        public override IJsonValue VisitArray(IJsonArray value)
        {
            if (value.Count > _index)
            {
                return value[_index];
            }
            return null;
        }
    }
}