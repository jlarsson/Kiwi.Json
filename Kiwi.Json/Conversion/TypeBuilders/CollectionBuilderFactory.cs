using System;
using System.Collections.Generic;
using System.Linq;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class CollectionBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            // Check which ICollection<T> is implemented
            var interfaceType = (from @interface in new[] {type}.Concat(type.GetInterfaces())
                                 where
                                     @interface.IsGenericType &&
                                     @interface.GetGenericTypeDefinition() == typeof (ICollection<>)
                                 select @interface)
                .FirstOrDefault();

            if (interfaceType == null)
            {
                // Check if it is IEnumerable<T>
                interfaceType = (from @interface in new[] {type}.Concat(type.GetInterfaces())
                                 where
                                     @interface.IsGenericType &&
                                     @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                                 select @interface)
                    .FirstOrDefault();
            }
            if (interfaceType == null)
            {
                return null;
            }

            var elementType = interfaceType.GetGenericArguments()[0];

            // Determine concrete ICollection<T> to instantiate
            var listType = type.IsInterface
                               ? typeof (List<>).MakeGenericType(elementType)
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
                 typeof (CollectionBuilderFactory<,>)
                     .MakeGenericType(listType, interfaceType.GetGenericArguments()[0])
                     .GetConstructor(Type.EmptyTypes)
                     .Invoke(new object[0])).CreateTypeBuilder(type);
        }

        #endregion
    }


    public class CollectionBuilderFactory<TCollection, TElem> : ITypeBuilderFactory
        where TCollection : class, ICollection<TElem>, new()
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            return new CollectionBuilder<TCollection, TElem>();
        }

        #endregion
    }
}