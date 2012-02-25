using Kiwi.Json.Serialization;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ConvertJsonToCustom: IJsonValueVisitor<object>
    {
        private readonly ITypeBuilder _typeBuilder;

        public ConvertJsonToCustom(ITypeBuilder typeBuilder)
        {
            _typeBuilder = typeBuilder;
        }

        public object VisitArray(IJsonArray value)
        {
            var a = _typeBuilder.CreateArray();
            foreach (var element in value)
            {
                a.AddElement(element.Visit(new ConvertJsonToCustom(a.GetElementBuilder())));
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
                o.SetMember(kv.Key, kv.Value.Visit(new ConvertJsonToCustom(o.GetMemberBuilder(kv.Key))));
            }
            return o.GetObject();
        }

        public object VisitString(IJsonString value)
        {
            return _typeBuilder.CreateString(value.Value);
        }
    }
}