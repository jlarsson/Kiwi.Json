using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.FromJson
{
    public class FromJsonToArray<TElem> : FromJsonToList<List<TElem>, TElem>
    {
        public FromJsonToArray(IFromJson fromJsonConverter) : base(fromJsonConverter)
        {
        }

        public override object FromJson(Type nativeType, IJsonValue value)
        {
            var l = (IList<TElem>) base.FromJson(nativeType, value);
            return l == null ? null : l.ToArray();
        }
    }
}