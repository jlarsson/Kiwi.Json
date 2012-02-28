using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ListBuilder<TList, TElem> : AbstractTypeBuilder, IArrayBuilder where TList : class, IList<TElem>, new()
    {
        protected ITypeBuilderRegistry Registry { get; private set; }
        private readonly TList _list = new TList();

        public ListBuilder(ITypeBuilderRegistry registry)
        {
            Registry = registry;
        }

        #region IArrayBuilder Members

        public override ITypeBuilder GetElementBuilder()
        {
            return Registry.GetTypeBuilder<TElem>();
        }

        public override void AddElement(object element)
        {
            _list.Add((TElem) element);
        }

        public override object GetArray()
        {
            return _list;
        }

        #endregion

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new TypeBuilderFactory()
                            {
                                OnCreateNull = () => null,
                                OnCreateArray = () => new ListBuilder<TList, TElem>(r)
                            };
        }
    }
}