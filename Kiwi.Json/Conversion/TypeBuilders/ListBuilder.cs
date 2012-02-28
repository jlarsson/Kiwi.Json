using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ListBuilder<TList, TElem> : AbstractTypeBuilder, IArrayBuilder where TList : class, IList<TElem>, new()
    {
        protected ITypeBuilderRegistry Registry { get; private set; }
        private TList _list;

        public ListBuilder(ITypeBuilderRegistry registry)
        {
            Registry = registry;
        }

        #region IArrayBuilder Members

        public ITypeBuilder GetElementBuilder()
        {
            return Registry.GetTypeBuilder<TElem>();
        }

        public void AddElement(object element)
        {
            (_list ?? (_list = new TList())).Add((TElem) element);
        }

        public virtual object GetArray()
        {
            return _list;
        }

        #endregion

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new ListBuilder<TList, TElem>(r);
        }

        public override object CreateNull()
        {
            return null;
        }

        public override IArrayBuilder CreateArray()
        {
            return new ListBuilder<TList, TElem>(Registry);
        }

        public override IObjectBuilder CreateObject()
        {
            return new DictionaryBuilder<Dictionary<string, object>, object>(Registry);
        }
    }
}