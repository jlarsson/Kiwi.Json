using System;
using Kiwi.Json.Conversion;
using Kiwi.Json.JPath;
using Kiwi.Json.Untyped;

namespace Kiwi.Json
{
    public static class JsonConvert
    {
        public static IJsonConvert Default { get; set; }
        static JsonConvert()
        {
            Default = new DefaultJsonConvert();
        }

        public static void RegisterCustomConverters(params IJsonConverter[] customConverters)
        {
            Default.RegisterCustomConverters(customConverters);
        }

        public static IJsonValue ToJson(object obj, params IJsonConverter[] customConverters)
        {
            return Default.ToJson(obj, customConverters);
        }

        public static object ToObject(this IJsonValue value, Type type, params IJsonConverter[] customConverters)
        {
            return Default.ToObject(type, value, customConverters);
        }

        public static T ToObject<T>(this IJsonValue value, params IJsonConverter[] customConverters)
        {
            return Default.ToObject<T>(value, customConverters);
        }

        public static IJsonValue MergeWith(this IJsonValue value, IJsonValue mergeWith)
        {
            return Default.MergeWith(value, mergeWith);
        }

        public static IJsonValue Parse(string json)
        {
            return Parse<IJsonValue>(json);
        }

        public static IJsonValue Parse(IJsonParser parser)
        {
            return Parse(parser, default(IJsonValue));
        }

        public static T Parse<T>(string json, params IJsonConverter[] customConverters)
        {
            return Parse(new JsonStringParser(json), default(T), customConverters);
        }
        public static object Parse(Type type, string json, params IJsonConverter[] customConverters)
        {
            return Parse(type, new JsonStringParser(json), null, customConverters);
        }

        public static object Parse(Type type, IJsonParser parser, object initializedInstance, params IJsonConverter[] customConverters)
        {
            return Default.Parse(type, parser, initializedInstance, customConverters);
        }

        public static T Parse<T>(string json, T initializedInstance, params IJsonConverter[] customConverters)
        {
            return Parse(new JsonStringParser(json), initializedInstance, customConverters);
        }

        public static T Parse<T>(IJsonParser parser, T initializedInstance, params IJsonConverter[] customConverters)
        {
            return Default.Parse<T>(parser, initializedInstance, customConverters);
        }

        public static string Write(object obj, params IJsonConverter[] customConverters)
        {
            var writer = new JsonStringWriter();
            Default.Write(obj, writer, customConverters);
            return writer.ToString();
        }

        public static void Write(object obj, IJsonWriter writer, params IJsonConverter[] customConverters)
        {
            Default.Write(obj, writer, customConverters);
        }

        public static IJsonPath ParseJsonPath(string jsonPath)
        {
            return new JsonPath(jsonPath);
        }
    }
}