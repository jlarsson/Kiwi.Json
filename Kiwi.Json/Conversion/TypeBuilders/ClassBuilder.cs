using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassBuilder<TClass> : AbstractTypeBuilder, IObjectBuilder where TClass : class // where TClass : new()
    {
// ReSharper disable StaticFieldInGenericType
        private static readonly Dictionary<string, ClassMember> MemberSetters = (from property in
                                                                                     typeof (TClass).GetProperties(
                                                                                         BindingFlags.GetProperty |
                                                                                         BindingFlags.SetProperty |
                                                                                         BindingFlags.Public |
                                                                                         BindingFlags.Instance)
                                                                                 where
                                                                                     (property.GetGetMethod().
                                                                                          GetParameters().Length == 0)
                                                                                 select new ClassMember
                                                                                            {
                                                                                                Name = property.Name,
                                                                                                Type =
                                                                                                    property.
                                                                                                    PropertyType,
                                                                                                Getter =
                                                                                                    new PropertyGetter(
                                                                                                    property),
                                                                                                Setter =
                                                                                                    new PropertySetter(
                                                                                                    property),
                                                                                            })
            .Union(
                from field in
                    typeof (TClass).GetFields(BindingFlags.GetField | BindingFlags.SetField | BindingFlags.Public |
                                              BindingFlags.Instance)
                select new ClassMember
                           {
                               Name = field.Name,
                               Type = field.FieldType,
                               Getter = new FieldGetter(field),
                               Setter = new FieldSetter(field),
                           })
            .ToDictionary(m => m.Name, m => m);


        private static readonly IClassActivator Activator = ClassActivator.Create(typeof (TClass));
// ReSharper restore StaticFieldInGenericType

        #region IObjectBuilder Members

        public override object CreateNewObject(ITypeBuilderRegistry registry, object instanceState)
        {
            if (instanceState is TClass)
            {
                return instanceState;
            }
            return Activator.CreateInstance();
        }

        public override object GetMemberState(string memberName, object @object)
        {
            ClassMember member;
            if (MemberSetters.TryGetValue(memberName, out member))
            {
                return member.Getter.GetMemberValue(@object);
            }
            return null;
        }

        public override ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            ClassMember member;
            if (MemberSetters.TryGetValue(memberName, out member))
            {
                return registry.GetTypeBuilder(member.Type);
            }
            return NothingBuilder.Instance;
        }

        public override void SetMember(string memberName, object @object, object value)
        {
            ClassMember member;
            if (MemberSetters.TryGetValue(memberName, out member))
            {
                member.Setter.SetValue(@object, value);
            }
        }

        public override object GetObject(object @object)
        {
            return @object;
        }

        #endregion

        public override IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        public override object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }

        #region Nested type: ClassMember

        protected class ClassMember
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public IMemberGetter Getter { get; set; }
            public IMemberSetter Setter { get; set; }
        }

        #endregion
    }
}