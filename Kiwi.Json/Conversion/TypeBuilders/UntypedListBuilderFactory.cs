using System;
using System.Collections;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class UntypedListBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            // Check if ICollection or IEnumerable is supported by type
            var interfaceType = typeof (IList).IsAssignableFrom(type)
                                    ? typeof (IList)
                                    : typeof (IEnumerable).IsAssignableFrom(type) ? typeof (IEnumerable) : null;
            if (interfaceType == null)
            {
                return null;
            }

            // Determine concrete ICollection to instantiate
            var listType = type.IsInterface
                               ? typeof (ArrayList)
                               : type;

            if (!type.IsAssignableFrom(listType))
            {
                return null;
            }

            // List must have default constructor
            if (listType.GetConstructor(Type.EmptyTypes) == null)
            {
                return null;
            }

            return
                ((ITypeBuilderFactory)
                 typeof (UntypedListBuilderFactory<>)
                     .MakeGenericType(listType)
                     .GetConstructor(Type.EmptyTypes)
                     .Invoke(new object[0])).CreateTypeBuilder(type);
        }

        #endregion
    }

    public class UntypedListBuilderFactory<TCollection> : ITypeBuilderFactory where TCollection : class, IList, new()
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            return new UntypedListBuilder<TCollection>();
        }

        #endregion
    }
}