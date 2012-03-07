using System;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumWriterFactory: ITypeWriterFactory
    {
        public Func<ITypeWriter> CreateTypeWriter(Type type, ITypeWriterRegistry registry)
        {
            if (type.IsEnum)
            {
                return
                    (Func<ITypeWriter>)
                    typeof(EnumWriter<>).MakeGenericType(type).GetMethod("CreateTypeWriterFactory",
                                                                         BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[] { registry });
            }
            return null;
        }
    }
}