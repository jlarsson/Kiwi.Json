using System;

namespace Kiwi.Json.Serialization.Serializers
{
    public interface ITypeSerializationContext
    {
        ITypeSerializer GetTypeSerializerForValue(object value);
        ITypeSerializer GetTypeSerializerForType(Type type);
    }
}