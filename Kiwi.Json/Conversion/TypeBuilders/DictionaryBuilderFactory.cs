using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryBuilderFactory: ITypeBuilderFactory
    {
        public Func<ITypeBuilder> CreateTypeBuilder(Type type, ITypeBuilderRegistry registry)
        {
            // Check which IDictionary<string,T> is implemented
            var valueType = (from @interface in new[] { type }.Concat(type.GetInterfaces())
                             where
                                 @interface.IsGenericType
                                 && @interface.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                                 && @interface.GetGenericArguments()[0] == typeof(string)
                             select @interface.GetGenericArguments()[1])
                .FirstOrDefault();
            if (valueType == null)
            {
                return null;
            }

            var concreteClass = type.IsClass ? type : typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType);

            if (!type.IsAssignableFrom(concreteClass))
            {
                return null;
            }

            var constructor = concreteClass.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
            {
                return () => new ClassWithoutDefaultConstructorBuilder(type);
            }

            return
                (Func<ITypeBuilder>)
                typeof(DictionaryBuilder<,>).MakeGenericType(concreteClass, valueType).GetMethod("CreateTypeBuilderFactory",
                                                                                                 BindingFlags.Static | BindingFlags.Public).
                    Invoke(null, new object[]{registry});
        }
    }
}