using System;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class ClassWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public Func<ITypeWriter> CreateTypeWriter(Type type)
        {
            if (type.IsClass)
            {
                return
                    (Func<ITypeWriter>)
                    typeof (ClassWriter<>).MakeGenericType(type).GetMethod("CreateTypeWriterFactory",
                                                                           BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[] {});
            }
            return null;
        }

        #endregion
    }
}