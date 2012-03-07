using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumWriter<TEnum>: ITypeWriter
    {
        public void Write(IJsonWriter writer, object value)
        {
            writer.WriteString(Enum.GetName(typeof (TEnum), value));
        }

        public static Func<ITypeWriter> CreateTypeWriterFactory(ITypeWriterRegistry registry)
        {
            return () => new EnumWriter<TEnum>();
        }

    }
}