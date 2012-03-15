using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumWriter<TEnum>: ITypeWriter
    {
        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            writer.WriteString(Enum.GetName(typeof (TEnum), value));
        }

        public static Func<ITypeWriter> CreateTypeWriterFactory()
        {
            return () => new EnumWriter<TEnum>();
        }
    }
}