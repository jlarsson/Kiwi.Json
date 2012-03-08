using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public class ConvertJsonValueToIndexNormalForm : IConvertJsonValueToIndexNormalForm
    {
        private static readonly IJsonValueVisitor<IJsonValue> Converter = new ConvertVisitor();

        private class ConvertVisitor : IJsonValueVisitor<IJsonValue>
        {
            public IJsonValue VisitArray(IJsonArray value)
            {
                return new JsonArray(from v in value select v.Visit(this));
            }

            public IJsonValue VisitBool(IJsonBool value)
            {
                return value;
            }

            public IJsonValue VisitDate(IJsonDate value)
            {
                return new JsonDate(value.Value.Date);
            }

            public IJsonValue VisitDouble(IJsonDouble value)
            {
                return value;
            }

            public IJsonValue VisitInteger(IJsonInteger value)
            {
                return value;
            }

            public IJsonValue VisitNull(IJsonNull value)
            {
                return value;
            }

            public IJsonValue VisitObject(IJsonObject value)
            {
                return new JsonObject(
                    (from kv in value
                     select new KeyValuePair<string, IJsonValue>(kv.Key, kv.Value.Visit(this)))
                        .ToDictionary(kv => kv.Key, kv => kv.Value)
                    );
            }

            public IJsonValue VisitString(IJsonString value)
            {
                return new JsonString(value.Value.ToLowerInvariant());
            }
        }

        public IJsonValue Convert(IJsonValue value)
        {
            return value.Visit(Converter);
        }
    }
}