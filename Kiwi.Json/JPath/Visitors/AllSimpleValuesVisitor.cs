using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Visitors
{
    internal class AllSimpleValuesVisitor: IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        public IEnumerable<IJsonValue> VisitArray(IJsonArray value)
        {
            return from element in value
                   from v in element.Visit(this)
                   select v;
        }

        public IEnumerable<IJsonValue> VisitBool(IJsonBool value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitDate(IJsonDate value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitDouble(IJsonDouble value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitInteger(IJsonInteger value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitNull(IJsonNull value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitObject(IJsonObject value)
        {
            return from member in value.Values
                from v in member.Visit(this)
                select v;
        }

        public IEnumerable<IJsonValue> VisitString(IJsonString value)
        {
            yield return value;
        }
    }
}