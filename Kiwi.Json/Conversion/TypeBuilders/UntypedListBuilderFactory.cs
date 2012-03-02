using System;
using System.Collections;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class UntypedListBuilderFactory: ITypeBuilderFactory
    {

        public Func<ITypeBuilder> CreateTypeBuilder(Type type, ITypeBuilderRegistry registry)
        {
            // Check if ICollection or IEnumerable is supported by type
            var interfaceType = typeof (IList).IsAssignableFrom(type) ? typeof (IList) : typeof(IEnumerable).IsAssignableFrom(type) ? typeof(IEnumerable) : null;
            if (interfaceType == null)
            {
                return null;
            }

            // Determine concrete ICollection to instantiate
            var listType = type.IsInterface
                               ? typeof(ArrayList)
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
                (Func<ITypeBuilder>)
                typeof(UntypedListBuilder<>).MakeGenericType(listType).GetMethod(
                    "CreateTypeBuilderFactory", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { registry });
        }
    }
}