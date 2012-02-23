using System.Collections.Generic;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class ListBuilder<TList,TElem>: AbstractTypeBuilder, IArrayBuilder where TList: class, IList<TElem>, new()
    {
        private readonly ITypeBuilderRegistry _registry;
        private TList _list;

        public ListBuilder(ITypeBuilderRegistry registry)
        {
            _registry = registry;
        }

        public override object CreateNull()
        {
            return null;
        }

        public override IArrayBuilder CreateArray()
        {
            _list = new TList();
            return this;
        }

        public ITypeBuilder GetElementBuilder()
        {
            return _registry.GetTypeBuilder<TElem>();
        }

        public void AddElement(object element)
        {
            _list.Add((TElem)element);
        }

        public virtual object GetObject()
        {
            return _list;
        }
    }
}
