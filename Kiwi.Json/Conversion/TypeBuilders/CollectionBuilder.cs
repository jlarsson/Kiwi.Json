using System;
using System.Collections;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class UntypedListBuilder<TCollection>: AbstractTypeBuilder, IArrayBuilder where TCollection: class, IList, new ()
    {
        private readonly ITypeBuilder _elementBuilder;

        public UntypedListBuilder(ITypeBuilder elementBuilder)
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

        public override object CreateNewArray(object instanceState)
        {
            if (instanceState is TCollection)
            {
                return instanceState;
            }
            return new TCollection();
        }

        public override ITypeBuilder GetElementBuilder()
        {
            return _elementBuilder;
        }

        public override void AddElement(object array, object element)
        {
            ((TCollection)array).Add(element);
        }

        public override object GetArray(object array)
        {
            return array;
        }

        #endregion

        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var builder = new UntypedListBuilder<TCollection>(registry.GetTypeBuilder<object>());

            return () => builder;
        }
    }
    public class CollectionBuilder<TCollection, TElem> : AbstractTypeBuilder, IArrayBuilder where TCollection : class, ICollection<TElem>, new()
    {
        private readonly ITypeBuilder _elementBuilder;

        public CollectionBuilder(ITypeBuilder elementBuilder)
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

        public override object CreateNewArray(object instanceState)
        {
            if (instanceState is TCollection)
            {
                return instanceState;
            }
            return new TCollection();
        }

        public override ITypeBuilder GetElementBuilder()
        {
            return _elementBuilder;
        }

        public override void AddElement(object array, object element)
        {
            ((TCollection)array).Add((TElem) element);
        }

        public override object GetArray(object array)
        {
            return array;
        }

        #endregion

        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var builder = new CollectionBuilder<TCollection, TElem>(registry.GetTypeBuilder<TElem>());

            return () => builder;
        }
    }
}