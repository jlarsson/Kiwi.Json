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

        public static ITypeWriterRegistry TypeWriterRegistry { get; set; }
        public static ITypeBuilderRegistry TypeBuilderRegistry { get; set; }

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

        public static T Parse<T>(IJsonParser parser, T initializedInstance)
        {
            return TypeBuilderRegistry.Read<T>(parser, initializedInstance);
        }

        public static T Parse<T>(string json)
        {
            return Parse(json, default(T));
        }

        public static T Parse<T>(string json, T initializedInstance)
        {
            return TypeBuilderRegistry.Read<T>(new JsonStringParser(json), initializedInstance);
        }

        public static string Write(object obj)
        {
            var writer = new JsonStringWriter();
            Write(obj, writer);
            return writer.ToString();
        }

        public static void Write(object obj, IJsonWriter writer)
        {
            TypeWriterRegistry.Write(writer, obj);
        }


        public static IJsonPath ParseJsonPath(string jsonPath)
        {
            return new JsonPath(jsonPath);
        }
    }
}