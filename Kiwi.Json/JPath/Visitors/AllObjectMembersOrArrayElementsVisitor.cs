using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Visitors
{
    internal class AllObjectMembersOrArrayElementsVisitor : IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        public IEnumerable<IJsonValue> VisitArray(IJsonArray value)
        {
            return value;
        }

        public IEnumerable<IJsonValue> VisitBool(IJsonBool value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitDate(IJsonDate value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitDouble(IJsonDouble value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitInteger(IJsonInteger value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitNull(IJsonNull value)
        {
            yield break;
        }

        public IEnumerable<IJsonValue> VisitObject(IJsonObject value)
        {
            return value.Values;
        }

        public IEnumerable<IJsonValue> VisitString(IJsonString value)
        {
            yield break;
        }
    }
}