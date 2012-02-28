using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassBuilder<TClass> : AbstractTypeBuilder, IObjectBuilder where TClass : new()
    {
        private readonly Dictionary<string, IMemberSetter> _memberSetters;
        private readonly ITypeBuilderRegistry _registry;
        private object _instance;

        protected ClassBuilder(ITypeBuilderRegistry registry, Dictionary<string, IMemberSetter> memberSetters)
        {
            _registry = registry;
            _memberSetters = memberSetters;
        }

        #region IObjectBuilder Members

        public ITypeBuilder GetMemberBuilder(string memberName)
        {
            var setter = default(IMemberSetter);
            if (_memberSetters.TryGetValue(memberName, out setter))
            {
                return _registry.GetTypeBuilder(setter.MemberType);
            }
            //return _registry.GetTypeBuilder<IJsonObject>();
            return NothingBuilder.Instance;
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

        #endregion

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            var memberSetters = (from property in
                                     typeof (TClass).GetProperties(
                                         BindingFlags.SetProperty | BindingFlags.Public |
                                         BindingFlags.Instance)
                                 where
                                     (property.GetGetMethod().GetParameters().Length == 0)
                                 select new PropertySetter(property) as IMemberSetter).
                Union(
                    from field in
                        typeof (TClass).GetFields(BindingFlags.SetField | BindingFlags.Public |
                                                  BindingFlags.Instance)
                    select new FieldSetter(field) as IMemberSetter).ToDictionary(v => v.MemberName,
                                                                                 v => v);


            return r => new ClassBuilder<TClass>(r, memberSetters);
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
    }
}