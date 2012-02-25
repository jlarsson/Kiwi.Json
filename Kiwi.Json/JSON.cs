using System.Collections.Generic;
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
            TypeWriterRegistry = new TypeWriterRegistry();
            TypeBuilderRegistry = new TypeBuilderRegistry();
        }

        public static IJsonConverter JsonConverter { get; private set; }
        public static ITypeWriterRegistry TypeWriterRegistry { get; set; }
        public static ITypeBuilderRegistry TypeBuilderRegistry { get; set; }

        public static IJsonValue ToJson(object obj)
        {
            //return JsonConverter.ToJson(obj);

            var writer = new ConstructJsonValueWriter();
            TypeWriterRegistry.GetTypeSerializerForValue(obj).Serialize(TypeWriterRegistry, writer, obj);
            return writer.GetValue();
        }

        public static T ToObject<T>(this IJsonValue value)
        {
            return (T)value.Visit(new ConvertJsonToCustomVisitor(TypeBuilderRegistry.GetTypeBuilder<T>()));
        }

        public static IJsonValue Read(string json)
        {
            return Read<IJsonValue>(json);
            //return (IJsonValue)new JsonStringReader(json).Parse(TypeBuilderRegistry.GetTypeBuilder<IJsonValue>());
        }

        public static T Read<T>(string json)
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

    public class ConstructJsonValueWriter: IJsonWriter
    {
        Stack<IJsonValue> _values = new Stack<IJsonValue>();
        Stack<string> _memberNames = new Stack<string>();

        public void WriteString(string value)
        {
            _values.Push(new JsonString(value));
        }

        public void WriteInteger(long value)
        {
            _values.Push(new JsonInteger(value));
        }

        public void WriteDouble(double value)
        {
            _values.Push(new JsonDouble(value));
        }

        public void WriteDate(System.DateTime value)
        {
            _values.Push(new JsonDate(value));
        }

        public void WriteBool(bool value)
        {
            _values.Push(new JsonBool(value));
        }

        public void WriteNull()
        {
            _values.Push(new JsonNull());
        }

        public void WriteArrayStart()
        {
            _values.Push(new JsonArray());
        }

        public void WriteArrayElementDelimiter()
        {
            var value = _values.Pop();
            (_values.Peek() as IJsonArray).Add(value);
        }

        public void WriteArrayEnd(int elementCount)
        {
            if (elementCount > 0)
            {
                var value = _values.Pop();
                (_values.Peek() as IJsonArray).Add(value);
            }
        }

        public void WriteObjectStart()
        {
            _values.Push(new JsonObject());
        }

        public void WriteMember(string memberName)
        {
            _memberNames.Push(memberName);
        }

        public void WriteObjectMemberDelimiter()
        {
            var name = _memberNames.Pop();
            var value = _values.Pop();

            (_values.Peek() as IJsonObject).Add(name, value);
        }

        public void WriteObjectEnd(int memberCount)
        {
            if (memberCount > 0)
            {
                var value = _values.Pop();
                var name = _memberNames.Pop();
                (_values.Peek() as IJsonObject).Add(name, value);
            }
        }

        public IJsonValue GetValue()
        {
            return _values.Peek();
        }
    }
}