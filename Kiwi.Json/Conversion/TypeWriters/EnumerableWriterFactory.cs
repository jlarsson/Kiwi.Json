using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumerableWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                    select ((ITypeWriterFactory)
                            typeof (EnumerableWriterFactory<>).MakeGenericType(@interface.GetGenericArguments()[0])
                                .GetConstructor(Type.EmptyTypes)
                                .Invoke(new object[0])).CreateTypeWriter(type)
                   )
                .Concat(
                    from @interface in type.GetInterfaces()
                    where @interface == typeof (IEnumerable)
                    select new EnumerableWriter()
                )
                .FirstOrDefault();
        }

        #endregion
    }

    public class EnumerableWriterFactory<T> : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            return new EnumerableWriter<T>();
        }

        #endregion
    }
}