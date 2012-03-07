using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public interface ITypeWriterRegistry
    {
        ITypeWriter GetTypeWriterForValue(object value);
        ITypeWriter GetTypeWriterForType(Type type);
    }
}