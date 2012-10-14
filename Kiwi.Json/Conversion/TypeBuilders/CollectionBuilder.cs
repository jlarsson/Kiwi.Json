using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class CollectionBuilder<TCollection, TElem> : AbstractTypeBuilder, IArrayBuilder
        where TCollection : class, ICollection<TElem>, new()
    {
        #region IArrayBuilder Members

        public override object CreateNewArray(ITypeBuilderRegistry registry, object instanceState)
        {
            if (instanceState is TCollection)
            {
                (instanceState as TCollection).Clear();
                return instanceState;
            }
            return new TCollection();
        }

        public override ITypeBuilder GetElementBuilder(ITypeBuilderRegistry registry)
        {
            return registry.GetTypeBuilder<TElem>();
        }

        public override void AddElement(object array, object element)
        {
            ((TCollection) array).Add((TElem) element);
        }

        public override object GetArray(object array)
        {
            return array;
        }

        #endregion

        public override IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        public override object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }

        protected override Type BuildType
        {
            get { return typeof (TCollection); }
        }
    }
}