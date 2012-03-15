using System;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class StructWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public Func<ITypeWriter> CreateTypeWriter(Type type)
        {
            if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
            {
                return
                    (Func<ITypeWriter>)
                    typeof (StructWriter<>).MakeGenericType(type).GetMethod("CreateTypeWriterFactory",
                                                                            BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[] {});
            }
            return null;
        }

        #endregion
    }
}