using Kiwi.Json.Conversion;
using Kiwi.Json.Serialization;
using Kiwi.Json.Serialization.Serializers;
using Kiwi.Json.Serialization.TypeBuilders;
using Kiwi.Json.Untyped;

namespace Kiwi.Json
{
    public static class JSON
    {
        static JSON()
        {
            JsonConverter = new JsonConverter();
            TypeSerializerRegistry = new TypeSerializerRegistry();
            TypeBuilderRegistry = new TypeBuilderRegistry();
        }

        public static IJsonConverter JsonConverter { get; private set; }
        public static ITypeSerializerRegistry TypeSerializerRegistry { get; set; }
        public static ITypeBuilderRegistry TypeBuilderRegistry { get; set; }

        public static IJsonValue Parse(string json)
        {
            return (IJsonValue)new JsonStringReader(json).Parse(TypeBuilderRegistry.GetTypeBuilder<IJsonValue>());
        }

        public static IJsonValue FromObject(object obj)
        {
            return JsonConverter.ToJson(obj);
        }

        public static T ToObject<T>(IJsonValue value)
        {
            return (T) JsonConverter.FromJson(typeof (T), value);
        }

        public static T ToObject<T>(string json)
        {
            return (T)new JsonStringReader(json).Parse(TypeBuilderRegistry.GetTypeBuilder<T>());
        }
    }
}