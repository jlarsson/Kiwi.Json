using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class StructBuilder<TStruct> : AbstractTypeBuilder, IObjectBuilder
    {
        private readonly Dictionary<string, StructMember> _members;

        private StructBuilder(Dictionary<string, StructMember> members)
        {
            _members = members;
        }

        #region IObjectBuilder Members

        public override IObjectBuilder CreateObjectBuilder()
        {
            return this;
        }

        public override object CreateNewObject()
        {
            return new Dictionary<string, object>();
        }

        public override ITypeBuilder GetMemberBuilder(string memberName)
        {
            StructMember member;
            if (_members.TryGetValue(memberName, out member))
            {
                return member.MemberBuilder;
            }
            return NothingBuilder.Instance;
        }

        public override void SetMember(string memberName, object @object, object value)
        {
            if (_members.ContainsKey(memberName))
            {
                ((Dictionary<string, object>)@object).Add(memberName, value);
            }
        }

        public override object GetObject(object @object)
        {
            var instanceMemberValues = (Dictionary<string, object>) @object;
            var membersToSet = (from kv in instanceMemberValues
                                select new
                                           {
                                               _members[kv.Key].Member,
                                               kv.Value
                                           }).ToArray();

            var members = (from v in membersToSet select v.Member).ToArray();
            var values = (from v in membersToSet select v.Value).ToArray();

            return
                FormatterServices.PopulateObjectMembers(FormatterServices.GetSafeUninitializedObject(typeof (TStruct)),
                                                        members, values);
        }

        #endregion

        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var members =
                (
                    from property in
                        typeof(TStruct).GetProperties(BindingFlags.SetProperty | BindingFlags.Public |
                                                       BindingFlags.Instance)
                    where (property.GetGetMethod().GetParameters().Length == 0)
                    select new StructMember
                    {
                        Name = property.Name,
                        //Member = property
                        //Member = typeof (TStruct).GetField(string.Format("<{0}>k__BackingField", property.Name),
                        //                              BindingFlags.NonPublic | BindingFlags.Instance),
                        Member = null,
                        MemberBuilder = null
                    })
                    .Concat(
                        from field in
                            typeof(TStruct).GetFields(BindingFlags.SetField | BindingFlags.Public |
                                                       BindingFlags.Instance)
                        select new StructMember
                        {
                            Name = field.Name,
                            Member = field,
                            MemberBuilder = registry.GetTypeBuilder(field.FieldType)
                        })
                    .Where(m => m.Member != null)
                    .ToDictionary(m => m.Name, m => m);

            var structBuilder = new StructBuilder<TStruct>(members);

            return () => structBuilder;
        }

        #region Nested type: StructMember

        private class StructMember
        {
            public string Name { get; set; }
            public MemberInfo Member { get; set; }
            public ITypeBuilder MemberBuilder { get; set; }
        }

        #endregion
    }
}