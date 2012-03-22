using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;
using Kiwi.Json.JPath;
using Kiwi.Json.Untyped;

namespace Kiwi.Json
{
    public static class JsonConvert
    {
        static JsonConvert()
        {
            TypeWriterRegistry = new TypeWriterRegistry();
            TypeBuilderRegistry = new TypeBuilderRegistry();
        }

        public static ICustomizableTypeWriterRegistry TypeWriterRegistry { get; set; }
        public static ICustomizableTypeBuilderRegistry TypeBuilderRegistry { get; set; }

        public static void RegisterCustomConverters(params IJsonConverter[] customConverters)
        {
            TypeWriterRegistry.RegisterConverters(customConverters);
            TypeBuilderRegistry.RegisterConverters(customConverters);
        }

        public static IJsonValue ToJson(object obj, params IJsonConverter[] customConverters)
        {
            var registry = CreateTypeWriterRegistry(customConverters);
            var writer = new ConstructJsonValue();
            registry.Write(writer, obj);
            return writer.GetValue();
        }

        private static ITypeWriterRegistry CreateTypeWriterRegistry(IJsonConverter[] customConverters)
        {
            return new CustomTypeWriterRegistry(TypeWriterRegistry, customConverters);
        }

        private static ITypeBuilderRegistry CreateTypeBuilderRegistry(IJsonConverter[] customConverters)
        {
            return new CustomTypeBuilderRegistry(TypeBuilderRegistry, customConverters);
        }

        public static IJsonValue ToJson(object obj)
        {
            var writer = new ConstructJsonValue();
            TypeWriterRegistry.Write(writer, obj);
            return writer.GetValue();
        }

        public static T ToObject<T>(this IJsonValue value)
        {
            var registry = TypeBuilderRegistry;
            return (T) value.Visit(new ConvertJsonToCustom(registry, registry.GetTypeBuilder<T>()));
        }

        public static IJsonValue MergeWith(this IJsonValue value, IJsonValue mergeWith)
        {
            var registry = TypeBuilderRegistry;
            mergeWith.Visit(new ConvertJsonToCustom(registry, registry.GetTypeBuilder(value.GetType()))
                                {InstanceState = value});
            return value;
        }

        public static IJsonValue Parse(string json)
        {
            return Parse<IJsonValue>(json);
        }

        public static IJsonValue Parse(IJsonParser parser)
        {
            return Parse(parser, default(IJsonValue));
        }

        public static T Parse<T>(IJsonParser parser)
        {
            return Parse(parser, default(T));
        }

        public static T Parse<T>(string json, params IJsonConverter[] customConverters)
        {
            return Parse(new JsonStringParser(json), default(T), customConverters);
        }

        public static T Parse<T>(string json, T initializedInstance, params IJsonConverter[] customConverters)
        {
            return Parse(new JsonStringParser(json), initializedInstance, customConverters);
        }
        public static T Parse<T>(IJsonParser parser, T initializedInstance, params IJsonConverter[] customConverters)
        {
            return Parse(parser, initializedInstance, CreateTypeBuilderRegistry(customConverters));
        }

        public static T Parse<T>(IJsonParser parser, T initializedInstance, ITypeBuilderRegistry registry)
        {
            return registry.Read<T>(parser, initializedInstance);
        }

        public static T Parse<T>(IJsonParser parser, T initializedInstance)
        {
            return TypeBuilderRegistry.Read<T>(parser, initializedInstance);
        }

        public static T Parse<T>(string json)
        {
            return Parse(default(T), json);
        }

        public static T Parse<T>(T initializedInstance, string json)
        {
            return TypeBuilderRegistry.Read<T>(new JsonStringParser(json), initializedInstance);
        }

        public static string Write(object obj)
        {
            var writer = new JsonStringWriter();
            Write(obj, writer, TypeWriterRegistry);
            return writer.ToString();
        }
        public static string Write(object obj, params IJsonConverter[] customConverters)
        {
            var writer = new JsonStringWriter();
            Write(obj, writer, CreateTypeWriterRegistry(customConverters));
            return writer.ToString();
        }

        public static void Write(object obj, IJsonWriter writer)
        {
            Write(obj, writer, TypeWriterRegistry);
        }

        public static void Write(object obj, IJsonWriter writer, ITypeWriterRegistry registry)
        {
            registry.Write(writer, obj);
        }

        public static IJsonPath ParseJsonPath(string jsonPath)
        {
            return new JsonPath(jsonPath);
        }
    }
}