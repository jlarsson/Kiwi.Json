using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class ClassWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            if ((type.IsClass) && !typeof(Delegate).IsAssignableFrom(type))
            {
                return
                    ((ITypeWriterFactory)
                     typeof (ClassWriterFactory<>)
                         .MakeGenericType(type)
                         .GetConstructor(Type.EmptyTypes)
                         .Invoke(new object[0])).CreateTypeWriter(type);
            }
            return null;
        }

        #endregion
    }

    public class ClassWriterFactory<TClass> : ITypeWriterFactory where TClass : class
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            return new ClassWriter<TClass>();
        }

        #endregion
    }
}