using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class ClassBuilder<TClass>: AbstractTypeBuilder, IObjectBuilder where TClass : new()
    {
        private readonly ITypeBuilderRegistry _registry;
        private object _instance;
        private readonly Dictionary<string, IMemberSetter> _memberSetters;

        public ClassBuilder(ITypeBuilderRegistry registry)
        {
            _registry = registry;

            _memberSetters = (from property in typeof (TClass).GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                                     where (property.GetGetMethod().GetParameters().Length == 0)
                                     select new PropertySetter(property) as IMemberSetter).Union(from field in typeof (TClass).GetFields(BindingFlags.SetField | BindingFlags.Public | BindingFlags.Instance)
                                                                                                 select new FieldSetter(field) as IMemberSetter).ToDictionary(v => v.MemberName, v => v);
        }

        public override object CreateNull()
        {
            return null;
        }

        public override IObjectBuilder CreateObject()
        {
            _instance = new TClass();
            return this;
        }

        public ITypeBuilder GetMemberBuilder(string memberName)
        {
            var setter = default(IMemberSetter);
            if (_memberSetters.TryGetValue(memberName, out setter))
            {
                return _registry.GetTypeBuilder(setter.MemberType);
            }
            return _registry.GetTypeBuilder<IJsonObject>();
        }

        public void SetMember(string memberName, object value)
        {
            var setter = default(IMemberSetter);
            if (_memberSetters.TryGetValue(memberName, out setter))
            {
                setter.SetValue(_instance, value);
            }
        }

        public object GetObject()
        {
            return _instance;
        }
    }
}