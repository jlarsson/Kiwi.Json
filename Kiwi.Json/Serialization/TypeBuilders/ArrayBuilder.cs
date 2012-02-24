using System;
using System.Collections.Generic;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class ArrayBuilder<TElem> : ListBuilder<List<TElem>, TElem>
    {
        public ArrayBuilder(ITypeBuilderRegistry registry) : base(registry)
        {
        }

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new ArrayBuilder<TElem>(r);
        }

        public override object GetObject()
        {
            object list = base.GetObject();
            return list == null ? null : ((List<TElem>) list).ToArray();
        }
    }
}