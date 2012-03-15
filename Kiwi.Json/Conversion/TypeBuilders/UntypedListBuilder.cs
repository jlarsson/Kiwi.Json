using System;
using System.Collections;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class UntypedListBuilder<TCollection>: AbstractTypeBuilder, IArrayBuilder where TCollection: class, IList, new ()
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
                return instanceState;
            }
            return new TCollection();
        }

        public override ITypeBuilder GetElementBuilder(ITypeBuilderRegistry registry)
        {
            return registry.GetTypeBuilder<object>();
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

        public static Func<ITypeBuilder> CreateTypeBuilderFactory()
        {
            var builder = new UntypedListBuilder<TCollection>();

            return () => builder;
        }
    }
}