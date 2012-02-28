using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public interface ITypeWriterRegistry
    {
        ITypeWriter GetTypeSerializerForValue(object value);
        ITypeWriter GetTypeSerializerForType(Type type);
    }
}