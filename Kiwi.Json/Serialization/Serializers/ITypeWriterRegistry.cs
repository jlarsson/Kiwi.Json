using System;

namespace Kiwi.Json.Serialization.Serializers
{
    public interface ITypeWriterRegistry
    {
        ITypeWriter GetTypeSerializerForValue(object value);
        ITypeWriter GetTypeSerializerForType(Type type);
    }
}