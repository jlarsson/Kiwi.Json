using System;
using System.Collections.Generic;
using System.Linq;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            // Check which IDictionary<K,V> is implemented
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

            var concreteClass = type.IsClass
                                    ? type
                                    : typeof(Dictionary<,>).MakeGenericType(kvTypes.KeyType, kvTypes.ValueType);

            if (!type.IsAssignableFrom(concreteClass))
            {
                return null;
            }

            return
                ((ITypeBuilderFactory)
                 typeof (DictionaryBuilderFactory<,,>)
                     .MakeGenericType(concreteClass, kvTypes.KeyType, kvTypes.ValueType)
                     .GetConstructor(Type.EmptyTypes)
                     .Invoke(new object[0])).CreateTypeBuilder(type);
        }

        #endregion
    }

    public class DictionaryBuilderFactory<TDictionary, TKey, TValue> : ITypeBuilderFactory
        where TDictionary : class, IDictionary<TKey, TValue>
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            return new DictionaryBuilder<TDictionary, TKey, TValue>();
        }

        #endregion
    }
}