using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class StructBuilder<TStruct> : AbstractTypeBuilder, IObjectBuilder
    {
        private readonly Dictionary<string, StructMember> _members;
        private readonly ITypeBuilderRegistry _registry;
        private Dictionary<string, object> _instanceMemberValues;

        public StructBuilder(ITypeBuilderRegistry registry)
        {
            _registry = registry;

            _members =
                (
                    from property in
                        typeof (TStruct).GetProperties(BindingFlags.SetProperty | BindingFlags.Public |
                                                       BindingFlags.Instance)
                    where (property.GetGetMethod().GetParameters().Length == 0)
                    select new StructMember
                               {
                                   Name = property.Name,
                                   Type = property.PropertyType,
                                   //Member = property
                                   //Member = typeof (TStruct).GetField(string.Format("<{0}>k__BackingField", property.Name),
                                   //                              BindingFlags.NonPublic | BindingFlags.Instance),
                                   Member = null
                               })
                    .Concat(
                        from field in
                            typeof (TStruct).GetFields(BindingFlags.SetField | BindingFlags.Public |
                                                       BindingFlags.Instance)
                        select new StructMember
                                   {
                                       Name = field.Name,
                                       Type = field.FieldType,
                                       Member = field,
                                   })
                    .Where(m => m.Member != null)
                    .ToDictionary(m => m.Name, m => m);
        }

        #region IObjectBuilder Members

        public override ITypeBuilder GetMemberBuilder(string memberName)
        {
            StructMember member;
            if (_members.TryGetValue(memberName, out member))
            {
                return _registry.GetTypeBuilder(member.Type);
            }
            return NothingBuilder.Instance;
        }

        public override void SetMember(string memberName, object value)
        {
            if (_members.ContainsKey(memberName))
            {
                _instanceMemberValues[memberName] = value;
            }
        }

        public override object GetObject()
        {
            var membersToSet = (from kv in _instanceMemberValues
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

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new StructBuilder<TStruct>(r);
        }

        public override IObjectBuilder CreateObject()
        {
            _instanceMemberValues = new Dictionary<string, object>();
            return this;
        }

        #region Nested type: StructMember

        private class StructMember
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public MemberInfo Member { get; set; }
        }

        #endregion
    }
}