﻿using Kiwi.Json.Conversion;
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
            TypeWriterRegistry = new TypeWriterRegistry();
            TypeBuilderRegistry = new TypeBuilderRegistry();
        }

        public static ITypeWriterRegistry TypeWriterRegistry { get; set; }
        public static ITypeBuilderRegistry TypeBuilderRegistry { get; set; }

        public static IJsonValue ToJson(object obj)
        {
            var writer = new ConstructJsonValue();
            TypeWriterRegistry.GetTypeSerializerForValue(obj).Serialize(writer, obj);
            return writer.GetValue();
        }

        public static T ToObject<T>(this IJsonValue value)
        {
            return (T)value.Visit(new ConvertJsonToCustom(TypeBuilderRegistry.GetTypeBuilder<T>()));
        }

        public static IJsonValue Read(string json)
        {
            return Read<IJsonValue>(json);
        }

        public static T Read<T>(string json)
        {
            return (T)new JsonStringReader(json).Parse(TypeBuilderRegistry.GetTypeBuilder<T>());
        }

        public static string Write(object obj)
        {
            var writer = new JsonStringWriter();
            TypeWriterRegistry.GetTypeSerializerForValue(obj).Serialize(writer, obj);
            return writer.ToString();
        }
    }
}