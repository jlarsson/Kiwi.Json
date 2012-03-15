using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class DictionaryWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public Func<ITypeWriter> CreateTypeWriter(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IDictionary<,>)
                          && @interface.GetGenericArguments()[0] == typeof (string)
                    let factory =
                        typeof (DictionaryWriter<>)
                        .MakeGenericType(@interface.GetGenericArguments()[1])
                        .GetMethod("CreateTypeWriterFactory", BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[] {})
                    select (Func<ITypeWriter>) factory
                   ).Concat(
                       from @interface in type.GetInterfaces()
                       where @interface == typeof (IDictionary)
                       select DictionaryWriter.CreateTypeWriterFactory()
                )
                .FirstOrDefault();
        }

        #endregion
    }
}