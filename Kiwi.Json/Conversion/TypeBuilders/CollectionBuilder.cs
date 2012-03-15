﻿using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class CollectionBuilder<TCollection, TElem> : AbstractTypeBuilder, IArrayBuilder where TCollection : class, ICollection<TElem>, new()
    {
        public override IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        #region IArrayBuilder Members

        public override object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }

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
            ((TCollection)array).Add((TElem) element);
        }

        public override object GetArray(object array)
        {
            return array;
        }

        #endregion

        public static Func<ITypeBuilder> CreateTypeBuilderFactory()
        {
            var builder = new CollectionBuilder<TCollection, TElem>();

            return () => builder;
        }
    }
}