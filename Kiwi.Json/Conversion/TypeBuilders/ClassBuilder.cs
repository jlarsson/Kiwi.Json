using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassBuilder<TClass> : AbstractTypeBuilder, IObjectBuilder where TClass: class // where TClass : new()
    {
        private readonly Dictionary<string, ClassMember> _memberSetters;
        private readonly IClassActivator _activator;

        private ClassBuilder(Dictionary<string, ClassMember> memberSetters, IClassActivator activator)
        {
            _memberSetters = memberSetters;
            _activator = activator;
        }

        #region IObjectBuilder Members

        public override IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        public override object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }

        public override object CreateNewObject(ITypeBuilderRegistry registry, object instanceState)
        {
            if (instanceState is TClass)
            {
                return instanceState;
            }
            return _activator.CreateInstance();
        }

        public override object GetMemberState(string memberName, object @object)
        {
            ClassMember member;
            if (_memberSetters.TryGetValue(memberName, out member))
            {
                return member.Getter.GetMemberValue(@object);
            }
            return null;
        }

        public override ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            ClassMember member;
            if (_memberSetters.TryGetValue(memberName, out member))
            {
                return registry.GetTypeBuilder(member.Type);
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

        public static Func<ITypeBuilder> CreateTypeBuilderFactory()
        {
            var memberSetters = (from property in
                                     typeof (TClass).GetProperties(
                                         BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public |
                                         BindingFlags.Instance)
                                 where
                                     (property.GetGetMethod().GetParameters().Length == 0)
                                 select new ClassMember
                                            {
                                                Name = property.Name,
                                                Type = property.PropertyType,
                                                Getter = new PropertyGetter(property),
                                                Setter = new PropertySetter(property),
                                            })
                .Union(
                    from field in
                        typeof(TClass).GetFields(BindingFlags.GetField | BindingFlags.SetField | BindingFlags.Public |
                                                  BindingFlags.Instance)
                    select new ClassMember
                               {
                                   Name = field.Name,
                                   Type = field.FieldType,
                                   Getter = new FieldGetter(field),
                                   Setter = new FieldSetter(field),
                               })
                .ToDictionary(m => m.Name, m => m);

            var classBuilder = new ClassBuilder<TClass>(memberSetters, ClassActivator.Create(typeof(TClass)));
            return () => classBuilder;
        }

        #region Nested type: ClassMember

        private class ClassMember
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public IMemberGetter Getter { get; set; }
            public IMemberSetter Setter { get; set; }
        }

        #endregion
    }
}