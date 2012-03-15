namespace Kiwi.Json.Conversion.TypeWriters
{
    public class SystemObjectWriter: ITypeWriter
    {
        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else if (value.GetType() != typeof(object))
            {
                registry.Write(writer, value);
            }
            else
            {
                writer.WriteObjectStart();
                writer.WriteObjectEnd(0);
            }
        }
    }
}