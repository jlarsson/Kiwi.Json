using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ListBuilder<TList, TElem> : AbstractTypeBuilder, IArrayBuilder where TList : class, IList<TElem>, new()
    {
        private readonly ITypeBuilder _elementBuilder;

        public ListBuilder(ITypeBuilder elementBuilder)
        {
            _elementBuilder = elementBuilder;
        }

        public override IArrayBuilder CreateArrayBuilder()
        {
            return this;
        }

        #region IArrayBuilder Members

        public override object CreateNull()
        {
            return null;
        }

        public override object CreateNewArray()
        {
            return new TList();
        }

        public override ITypeBuilder GetElementBuilder()
        {
            return _elementBuilder;
        }

        public override void AddElement(object array, object element)
        {
            ((TList)array).Add((TElem) element);
        }

        public override object GetArray(object array)
        {
            return array;
        }

        #endregion

        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var builder = new ListBuilder<TList, TElem>(registry.GetTypeBuilder<TElem>());

            return () => builder;
        }
    }
}