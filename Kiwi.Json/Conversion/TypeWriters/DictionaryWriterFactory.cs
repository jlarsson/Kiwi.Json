using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class DictionaryWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IDictionary<,>)
                          && @interface.GetGenericArguments()[0] == typeof (string)
                    select ((ITypeWriterFactory)
                            typeof (DictionaryWriterFactory<>).MakeGenericType(@interface.GetGenericArguments()[1])
                                .GetConstructor(Type.EmptyTypes)
                                .Invoke(new object[0])).CreateTypeWriter(type)
                   ).Concat(
                       from @interface in type.GetInterfaces()
                       where @interface == typeof (IDictionary)
                       select new DictionaryWriter()
                )
                .FirstOrDefault();
        }

        #endregion
    }

    public class DictionaryWriterFactory<TValue> : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            return new DictionaryWriter<TValue>();
        }

        #endregion
    }
}