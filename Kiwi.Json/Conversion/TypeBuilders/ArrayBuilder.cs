using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ArrayBuilder<TElem> : CollectionBuilder<List<TElem>, TElem>
    {
        public ArrayBuilder(ITypeBuilder elementBuilder)
            : base(elementBuilder)
        {
        }

        public new static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var arrayBuilder = new ArrayBuilder<TElem>(registry.GetTypeBuilder<TElem>());
            return () => arrayBuilder;

        }

        public override object GetArray(object array)
        {
            var list = base.GetArray(array);
            return list == null ? null : ((List<TElem>) list).ToArray();
        }
    }
}