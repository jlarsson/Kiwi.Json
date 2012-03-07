using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumerableWriterFactory: ITypeWriterFactory
    {
        public Func<ITypeWriter> CreateTypeWriter(Type type, ITypeWriterRegistry registry)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    let factory = typeof(EnumerableWriter<>).MakeGenericType(@interface.GetGenericArguments()[0])
                        .GetMethod("CreateTypeWriterFactory", BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[]{registry})
                    select (Func<ITypeWriter>)factory
                   )
                .Concat(
                    from @interface in type.GetInterfaces()
                    where @interface == typeof(IEnumerable)
                    select EnumerableWriter.CreateTypeWriterFactory(registry)
                )
                .FirstOrDefault();
        }
    }
}