using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.JPath.Visitors
{
    internal class AllObjectValuesVisitor : IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        public IEnumerable<IJsonValue> VisitArray(IJsonArray value)
        {
            return from elem in value
                   from v in elem.Visit(this)
                   select v;
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
            yield return value;
            foreach (var v in from member in value.Values
                                     from v in member.Visit(this)
                                     select v)
            {
                yield return v;
            }

        }

        public IEnumerable<IJsonValue> VisitString(IJsonString value)
        {
            yield break;
        }
    }
}