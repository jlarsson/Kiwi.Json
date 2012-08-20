using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassBuilder<TClass> : AbstractTypeBuilder where TClass : class
    {
// ReSharper disable StaticFieldInGenericType
        private static readonly Dictionary<string, ClassMember> Members;

        private static readonly IClassActivator Activator;
// ReSharper restore StaticFieldInGenericType

        static ClassBuilder()
        {
            Members = DiscoverMembers();
            Activator = ClassActivator.Create(typeof(TClass));
        }

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
            if (Members.TryGetValue(memberName, out member))
            {
                return member.Getter.GetMemberValue(@object);
            }
            return null;
        }

        public override ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            ClassMember member;
            if (Members.TryGetValue(memberName, out member))
            {
                return registry.GetTypeBuilder(member.Type);
            }
            return NothingBuilder.Instance;
        }

        public override void SetMember(string memberName, object @object, object value)
        {
            ClassMember member;
            if (Members.TryGetValue(memberName, out member))
            {
                member.Setter.SetValue(@object, value);
            }
        }

        public override object GetObject(object @object)
        {
            return @object;
        }

        #endregion

        private static Attribute GetDataMemberAttribute(MemberInfo member)
        {
            return
                member.GetCustomAttributes(true).OfType<Attribute>().FirstOrDefault(
                    a => a.GetType().FullName == "System.Runtime.Serialization.DataMemberAttribute");
        }

        private static string GetMemberName(MemberInfo member, Attribute dataMemberAttribute)
        {
            var name = dataMemberAttribute == null
                           ? null
                           : (string)
                             dataMemberAttribute.GetType().GetProperty("Name").GetValue(dataMemberAttribute,
                                                                                        new object[0]);
            return name ?? member.Name;
        }

        private static Dictionary<string, ClassMember> DiscoverMembers()
        {
            var classUsesDataContract =
                (typeof (TClass).GetCustomAttributes(true).OfType<Attribute>().Where(
                    a => a.GetType().FullName == "System.Runtime.Serialization.DataContractAttribute")).Any();

            return (from property in typeof (TClass).GetProperties(
                BindingFlags.GetProperty
                | BindingFlags.SetProperty
                | BindingFlags.Public
                | BindingFlags.Instance)
                    where property.CanRead && property.CanWrite
                    where (property.GetGetMethod().GetParameters().Length == 0)

                    let getter = property.GetGetMethod()
                    let setter = property.GetSetMethod(true)
                    where getter != null
                    where setter != null
                    where getter.IsPublic
                    where setter.IsPublic

                    let dm = classUsesDataContract ? GetDataMemberAttribute(property) : null
                    where classUsesDataContract == (dm != null)
                    select new ClassMember
                               {
                                   Name = GetMemberName(property, dm),
                                   Type = property.PropertyType,
                                   Getter = new PropertyGetter(property),
                                   Setter = new PropertySetter(property),
                               })
                .Union(
                    from field in typeof (TClass).GetFields(
                        BindingFlags.GetField | BindingFlags.SetField | BindingFlags.Public |
                        BindingFlags.Instance)
                    let dm = classUsesDataContract ? GetDataMemberAttribute(field) : null
                    where classUsesDataContract == (dm != null)
                    select new ClassMember
                               {
                                   Name = GetMemberName(field, dm),
                                   Type = field.FieldType,
                                   Getter = new FieldGetter(field),
                                   Setter = new FieldSetter(field),
                               })
                .ToDictionary(m => m.Name, m => m);
        }

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