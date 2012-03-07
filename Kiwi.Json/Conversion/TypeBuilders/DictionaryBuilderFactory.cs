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
            var kvTypes = (from @interface in new[] {type}.Concat(type.GetInterfaces())
                           where
                               @interface.IsGenericType
                               && @interface.GetGenericTypeDefinition() == typeof (IDictionary<,>)
                           select new
                                      {
                                          KeyType = @interface.GetGenericArguments()[0],
                                          ValueType = @interface.GetGenericArguments()[1]
                                      }).FirstOrDefault();
            if (kvTypes == null)
            {
                return null;
            }

            if (kvTypes.KeyType != typeof(string))
            {
                return () => new DictionaryWithWrongKeyTypeBuilder(kvTypes.KeyType);

            }

            var concreteClass = type.IsClass ? type : typeof(Dictionary<,>).MakeGenericType(typeof(string), kvTypes.ValueType);

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
                typeof(DictionaryBuilder<,>).MakeGenericType(concreteClass, kvTypes.ValueType).GetMethod("CreateTypeBuilderFactory",
                                                                                                 BindingFlags.Static | BindingFlags.Public).
                    Invoke(null, new object[]{registry});
        }
    }
}