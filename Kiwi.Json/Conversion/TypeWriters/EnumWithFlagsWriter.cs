namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumWithFlagsWriter<TEnum> : ITypeWriter where TEnum : struct
    {
        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            writer.WriteInteger((int) value);
        }

        #endregion

    }
}