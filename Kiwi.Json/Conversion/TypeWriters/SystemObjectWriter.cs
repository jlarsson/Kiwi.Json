namespace Kiwi.Json.Conversion.TypeWriters
{
    public class SystemObjectWriter: ITypeWriter
    {
        private readonly ITypeWriterRegistry _registry;

        public SystemObjectWriter(ITypeWriterRegistry registry)
        {
            _registry = registry;
        }

        public void Write(IJsonWriter writer, object value)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else if (value.GetType() != typeof(object))
            {
                _registry.GetTypeWriterForValue(value).Write(writer, value);
            }
            else
            {
                writer.WriteObjectStart();
                writer.WriteObjectEnd(0);
            }
        }
    }
}