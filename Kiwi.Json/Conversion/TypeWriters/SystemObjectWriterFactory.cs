using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class SystemObjectWriterFactory: ITypeWriterFactory
    {
        public Func<ITypeWriter> CreateTypeWriter(Type type, ITypeWriterRegistry registry)
        {
            return type == typeof (object) ? (Func<ITypeWriter>)(() => new SystemObjectWriter(registry)) : null;
        }
    }
}