using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public interface ITypeWriterFactory
    {
        Func<ITypeWriter> CreateTypeWriter(Type type, ITypeWriterRegistry registry);
    }
}