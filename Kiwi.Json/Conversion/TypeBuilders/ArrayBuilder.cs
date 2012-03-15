using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ArrayBuilder<TElem> : CollectionBuilder<List<TElem>, TElem>
    {
        public override object GetArray(object array)
        {
            var list = base.GetArray(array);
            return list == null ? null : ((List<TElem>) list).ToArray();
        }
    }
}