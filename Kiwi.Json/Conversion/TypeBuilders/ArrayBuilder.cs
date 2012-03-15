using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ArrayBuilder<TElem> : CollectionBuilder<List<TElem>, TElem>
    {
        public new static Func<ITypeBuilder> CreateTypeBuilderFactory()
        {
            var arrayBuilder = new ArrayBuilder<TElem>();
            return () => arrayBuilder;

        }

        public override object GetArray(object array)
        {
            var list = base.GetArray(array);
            return list == null ? null : ((List<TElem>) list).ToArray();
        }
    }
}