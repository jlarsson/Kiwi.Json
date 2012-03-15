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

            if (kvTypes.KeyType != typeof (string))
            {
                return new DictionaryWithWrongKeyTypeBuilder(kvTypes.KeyType);
            }

            var concreteClass = type.IsClass
                                    ? type
                                    : typeof (Dictionary<,>).MakeGenericType(typeof (string), kvTypes.ValueType);

            if (!type.IsAssignableFrom(concreteClass))
            {
                return null;
            }

            return
                ((ITypeBuilderFactory)
                 typeof (DictionaryBuilderFactory<,>)
                     .MakeGenericType(concreteClass, kvTypes.ValueType)
                     .GetConstructor(Type.EmptyTypes)
                     .Invoke(new object[0])).CreateTypeBuilder(type);
        }

        #endregion
    }

    public class DictionaryBuilderFactory<TDictionary, TValue> : ITypeBuilderFactory
        where TDictionary : class, IDictionary<string, TValue>
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            return new DictionaryBuilder<TDictionary, TValue>();
        }

        #endregion
    }
}