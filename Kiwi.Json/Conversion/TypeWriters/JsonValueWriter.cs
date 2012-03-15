using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class JsonValueWriter : ITypeWriter
    {
        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            ((IJsonValue) value).Write(writer);
        }

        #endregion
    }
}