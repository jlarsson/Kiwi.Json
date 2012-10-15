namespace Kiwi.Json.Conversion.TypeWriters
{
    public class NothingTypeWriter : ITypeWriter
    {
        public static readonly ITypeWriter Instance = new NothingTypeWriter();

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            writer.WriteNull();
        }

        #endregion
    }
}