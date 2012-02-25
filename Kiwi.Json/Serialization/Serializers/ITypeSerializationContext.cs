using System;

namespace Kiwi.Json.Serialization.Serializers
{
    public interface ITypeSerializationContext
    {
        ITypeWriter GetTypeSerializerForValue(object value);
        ITypeWriter GetTypeSerializerForType(Type type);
    }
}