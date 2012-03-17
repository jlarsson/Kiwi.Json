using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class StructWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
            {
                return
                    ((ITypeWriterFactory)
                     typeof (StructWriterFactory<>)
                         .MakeGenericType(type)
                         .GetConstructor(Type.EmptyTypes)
                         .Invoke(new object[0])).CreateTypeWriter(type);
            }
            return null;
        }

        #endregion
    }

    public class StructWriterFactory<TStruct> : ITypeWriterFactory where TStruct : struct
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            return new StructWriter<TStruct>();
        }

        #endregion
    }
}