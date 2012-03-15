using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ConvertJsonToCustom : IJsonValueVisitor<object>
    {
        private readonly ITypeBuilder _typeBuilder;

        public ConvertJsonToCustom(ITypeBuilder typeBuilder)
        {
            _typeBuilder = typeBuilder;
        }

        public object InstanceState { get; set; }

        #region IJsonValueVisitor<object> Members

        public object VisitArray(IJsonArray value)
        {
            var arrayBuilder = _typeBuilder.CreateArrayBuilder();
            var array = arrayBuilder.CreateNewArray(InstanceState);
            foreach (var element in value)
            {
                arrayBuilder.AddElement(array, element.Visit(new ConvertJsonToCustom(arrayBuilder.GetElementBuilder())));
            }
            return arrayBuilder.GetArray(array);
        }

        public object VisitBool(IJsonBool value)
        {
            return _typeBuilder.CreateBool(value.Value);
        }

        public object VisitDate(IJsonDate value)
        {
            return _typeBuilder.CreateDateTime(value.Value, null);
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
            var objectBuilder = _typeBuilder.CreateObjectBuilder();
            var @object = objectBuilder.CreateNewObject(InstanceState);
            foreach (var kv in value)
            {
                var memberState = objectBuilder.GetMemberState(kv.Key, @object);
                objectBuilder.SetMember(kv.Key, @object,
                                        kv.Value.Visit(new ConvertJsonToCustom(objectBuilder.GetMemberBuilder(kv.Key))
                                                           {InstanceState = memberState}));
            }
            return objectBuilder.GetObject(@object);
        }

        public object VisitString(IJsonString value)
        {
            return _typeBuilder.CreateString(value.Value);
        }

        #endregion
    }
}