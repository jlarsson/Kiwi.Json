using System;
using System.Collections.Generic;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class ArrayBuilder<TElem> : ListBuilder<List<TElem>, TElem>
    {
        public ArrayBuilder(ITypeBuilderRegistry registry) : base(registry)
        {
        }

        public new static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new ArrayBuilder<TElem>(r);
        }

        public override object GetArray()
        {
            object list = base.GetArray();
            return list == null ? null : ((List<TElem>) list).ToArray();
        }
    }
}