using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public class ExtractFilterValuesVisitor : IJsonValueVisitor<IEnumerable<IJsonValue>>
    {
        public IEnumerable<IJsonValue> VisitArray(IJsonArray value)
        {
            return from elem in value
                   from filterValue in elem.Visit(this)
                   select filterValue;
        }

        public IEnumerable<IJsonValue> VisitBool(IJsonBool value)
        {
            yield return value;
        }

        public IEnumerable<IJsonValue> VisitDate(IJsonDate value)
        {
            yield return new JsonDate(value.Value.Date);
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
            yield break;
        }

        public IEnumerable<IJsonValue> VisitString(IJsonString value)
        {
            yield return new JsonString(value.Value.ToLowerInvariant());
        }
    }
}