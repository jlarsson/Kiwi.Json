using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class ConvertJsonToCustom : IJsonValueVisitor<object>
    {
        private readonly ITypeBuilderRegistry _registry;
        private readonly ITypeBuilder _typeBuilder;

        public ConvertJsonToCustom(ITypeBuilderRegistry registry, ITypeBuilder typeBuilder)
        {
            _registry = registry;
            _typeBuilder = typeBuilder;
        }

        public object InstanceState { get; set; }

        #region IJsonValueVisitor<object> Members

        public object VisitArray(IJsonArray value)
        {
            var arrayBuilder = _typeBuilder.CreateArrayBuilder(_registry);
            var array = arrayBuilder.CreateNewArray(_registry, InstanceState);
            foreach (var element in value)
            {
                arrayBuilder.AddElement(array, element.Visit(new ConvertJsonToCustom(_registry, arrayBuilder.GetElementBuilder(_registry))));
            }
            return arrayBuilder.GetArray(array);
        }

        public object VisitBool(IJsonBool value)
        {
            return _typeBuilder.CreateBool(_registry, value.Value);
        }

        public object VisitDate(IJsonDate value)
        {
            return _typeBuilder.CreateDateTime(_registry, value.Value, null);
        }

        public object VisitDouble(IJsonDouble value)
        {
            return _typeBuilder.CreateNumber(_registry, value.Value);
        }

        public object VisitInteger(IJsonInteger value)
        {
            return _typeBuilder.CreateNumber(_registry, value.Value);
        }

        public object VisitNull(IJsonNull value)
        {
            return _typeBuilder.CreateNull(_registry);
        }

        public object VisitObject(IJsonObject value)
        {
            var objectBuilder = _typeBuilder.CreateObjectBuilder(_registry);
            var @object = objectBuilder.CreateNewObject(_registry, InstanceState);
            foreach (var kv in value)
            {
                var memberState = objectBuilder.GetMemberState(kv.Key, @object);
                objectBuilder.SetMember(kv.Key, @object,
                                        kv.Value.Visit(new ConvertJsonToCustom(_registry, objectBuilder.GetMemberBuilder(_registry, kv.Key))
                                                           {InstanceState = memberState}));
            }
            return objectBuilder.GetObject(@object);
        }

        public object VisitString(IJsonString value)
        {
            return _typeBuilder.CreateString(_registry, value.Value);
        }

        #endregion
    }
}