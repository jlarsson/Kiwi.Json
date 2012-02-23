using System.Collections.Generic;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class ArrayBuilder<TElem>: ListBuilder<List<TElem>,TElem>
    {
        public ArrayBuilder(ITypeBuilderRegistry registry) : base(registry)
        {
        }

        public override object GetObject()
        {
            var list = base.GetObject();
            return list == null ? null : ((List<TElem>) list).ToArray();
        }
    }
}