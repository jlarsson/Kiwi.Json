using System.IO;
using Kiwi.Json.Conversion;
using Kiwi.Json.Serialization;
using Kiwi.Json.Untyped;

namespace Kiwi.Json
{
    public static class JSON
    {
        static JSON()
        {
            DefaultJsonConverter = new JsonConverter();
        }

        public static IJsonConverter DefaultJsonConverter { get; private set; }

        public static IJsonValue Parse(string json)
        {
            return new JsonStringDeserializer(json).Parse();
        }

        public static IJsonValue FromObject(object obj)
        {
            return DefaultJsonConverter.ToJson(obj);
        }

        public static T ToObject<T>(IJsonValue value)
        {
            return (T) DefaultJsonConverter.FromJson(typeof (T), value);
        }

        public static T ToObject<T>(string json)
        {
            return ToObject<T>(new JsonStringDeserializer(json).Parse());
        }

        public static byte[] ToBinary(IJsonValue value)
        {
            using (var stream = new MemoryStream())
            {
                WriteBinary(value, stream);
                return stream.ToArray();
            }
        }

        public static void WriteBinary(IJsonValue value, Stream stream)
        {
            var writer = new BinaryWriter(stream);
            WriteBinary(value, writer);
            writer.Flush();
        }

        public static void WriteBinary(IJsonValue value, BinaryWriter writer)
        {
            value.Visit(new BinarySerializer(writer));
        }

        public static IJsonValue FromBinary(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes, false))
            {
                return ReadBinary(stream);
            }
        }

        public static IJsonValue ReadBinary(Stream stream)
        {
            return ReadBinary(new BinaryReader(stream));
        }

        public static IJsonValue ReadBinary(BinaryReader reader)
        {
            return new BinaryDeserializer(reader).Read();
        }
    }
}