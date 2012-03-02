using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassBuilder<TClass> : AbstractTypeBuilder, IObjectBuilder where TClass : new()
    {
        private readonly Dictionary<string, ClassMember> _memberSetters;

        private ClassBuilder(Dictionary<string, ClassMember> memberSetters)
        {
            _memberSetters = memberSetters;
        }

        #region IObjectBuilder Members

        public override IObjectBuilder CreateObjectBuilder()
        {
            return this;
        }

        public override object CreateNull()
        {
            return null;
        }

        public override object CreateNewObject()
        {
            return new TClass();
        }

        public override ITypeBuilder GetMemberBuilder(string memberName)
        {
            ClassMember member;
            if (_memberSetters.TryGetValue(memberName, out member))
            {
                return member.MemberBuilder;
            }
            return NothingBuilder.Instance;
        }

        public override void SetMember(string memberName, object @object, object value)
        {
            ClassMember member;
            if (_memberSetters.TryGetValue(memberName, out member))
            {
                member.Setter.SetValue(@object, value);
            }
        }

        public override object GetObject(object @object)
        {
            return @object;
        }

        #endregion

        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var memberSetters = (from property in
                                     typeof (TClass).GetProperties(
                                         BindingFlags.SetProperty | BindingFlags.Public |
                                         BindingFlags.Instance)
                                 where
                                     (property.GetGetMethod().GetParameters().Length == 0)
                                 select new ClassMember
                                            {
                                                Name = property.Name,
                                                Setter = new PropertySetter(property),
                                                MemberBuilder = registry.GetTypeBuilder(property.PropertyType)
                                            })
                .Union(
                    from field in
                        typeof (TClass).GetFields(BindingFlags.SetField | BindingFlags.Public |
                                                  BindingFlags.Instance)
                    select new ClassMember
                               {
                                   Name = field.Name,
                                   Setter = new FieldSetter(field),
                                   MemberBuilder = registry.GetTypeBuilder(field.FieldType)
                               })
                .ToDictionary(m => m.Name, m => m);

            var classBuilder = new ClassBuilder<TClass>(memberSetters);
            return () => classBuilder;
        }

        #region Nested type: ClassMember

        private class ClassMember
        {
            public string Name { get; set; }
            public IMemberSetter Setter { get; set; }
            public ITypeBuilder MemberBuilder { get; set; }
        }

        #endregion
    }
}