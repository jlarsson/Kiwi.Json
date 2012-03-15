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
            mergeWith.Visit(new ConvertJsonToCustom(registry, registry.GetTypeBuilder(value.GetType())){InstanceState = value});
            return value;
        }

        public static IJsonValue Read(string json)
        {
            return Read<IJsonValue>(json);
        }

        public static IJsonValue Read(IJsonReader reader)
        {
            return Read(reader, default(IJsonValue));
        }


        public static T Read<T>(IJsonReader reader)
        {
            return Read(reader, default(T));
        }

        public static T Read<T>(IJsonReader reader, T initializedInstance)
        {
            return TypeBuilderRegistry.Read<T>(reader, initializedInstance);
        }

        public static T Read<T>(string json)
        {
            return Read(json, default(T));
        }

        public static T Read<T>(string json, T initializedInstance)
        {
            return TypeBuilderRegistry.Read<T>(new JsonStringReader(json), initializedInstance);
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