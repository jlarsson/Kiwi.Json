using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            if (type.IsEnum)
            {
                return
                    ((ITypeWriterFactory)
                     typeof (EnumWriterFactory<>)
                         .MakeGenericType(type)
                         .GetConstructor(Type.EmptyTypes)
                         .Invoke(new object[0])).CreateTypeWriter(type);
            }
            return null;
        }

        #endregion
    }

    public class EnumWriterFactory<TEnum> : ITypeWriterFactory where TEnum : struct
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            return new EnumWriter<TEnum>();
        }

        #endregion
    }
}