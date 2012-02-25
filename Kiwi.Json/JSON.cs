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
            //return (T) JsonConverter.FromJson(typeof (T), value);
            return (T)value.Visit(new ConvertJsonToCustomVisitor(TypeBuilderRegistry.GetTypeBuilder<T>()));
        }

        public static T ToObject<T>(string json)
        {
            return (T)new JsonStringReader(json).Parse(TypeBuilderRegistry.GetTypeBuilder<T>());
        }
    }

    public class ConvertJsonToCustomVisitor: IJsonValueVisitor<object>
    {
        private readonly ITypeBuilder _typeBuilder;

        public ConvertJsonToCustomVisitor(ITypeBuilder typeBuilder)
        {
            _typeBuilder = typeBuilder;
        }

        public object VisitArray(IJsonArray value)
        {
            var a = _typeBuilder.CreateArray();
            foreach (var element in value)
            {
                a.AddElement(element.Visit(new ConvertJsonToCustomVisitor(a.GetElementBuilder())));
            }
            return a.GetArray();
        }

        public object VisitBool(IJsonBool value)
        {
            return _typeBuilder.CreateBool(value.Value);
        }

        public object VisitDate(IJsonDate value)
        {
            return _typeBuilder.CreateDateTime(value.Value);
        }

        public object VisitDouble(IJsonDouble value)
        {
            return _typeBuilder.CreateNumber(value.Value);
        }

        public object VisitInteger(IJsonInteger value)
        {
            return _typeBuilder.CreateNumber(value.Value);
        }

        public object VisitNull(IJsonNull value)
        {
            return _typeBuilder.CreateNull();
        }

        public object VisitObject(IJsonObject value)
        {
            var o = _typeBuilder.CreateObject();
            foreach (var kv in value)
            {
                o.SetMember(kv.Key, kv.Value.Visit(new ConvertJsonToCustomVisitor(o.GetMemberBuilder(kv.Key))));
            }
            return o.GetObject();
        }

        public object VisitString(IJsonString value)
        {
            return _typeBuilder.CreateString(value.Value);
        }
    }
}