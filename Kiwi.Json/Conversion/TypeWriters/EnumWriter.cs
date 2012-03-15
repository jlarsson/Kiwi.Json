using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumWriter<TEnum> : ITypeWriter
    {
        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            writer.WriteString(Enum.GetName(typeof (TEnum), value));
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory()
        {
            return () => new EnumWriter<TEnum>();
        }
    }
}