using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public interface ITypeWriterFactory
    {
        ITypeWriter CreateTypeWriter(Type type);
    }
}