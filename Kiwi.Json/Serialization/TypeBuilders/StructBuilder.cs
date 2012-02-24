using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class StructBuilder<TStruct> : AbstractTypeBuilder, IObjectBuilder
    {
        private readonly ITypeBuilderRegistry _registry;
        private readonly Dictionary<string, StructMember> _members;
        private Dictionary<string, object> _instanceMemberValues;

        private class StructMember
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public MemberInfo Member { get; set; }
        }

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
                                   Member =
                                       typeof (TStruct).GetField(string.Format("<{0}>k__BackingField", property.Name),
                                                                 BindingFlags.NonPublic | BindingFlags.Instance)
                               })
                    .Concat(
                        from field in
                            typeof (TStruct).GetFields(BindingFlags.SetField | BindingFlags.Public |
                                                       BindingFlags.Instance)
                        select new StructMember()
                                   {
                                       Name = field.Name,
                                       Type = field.FieldType,
                                       Member = field
                                   })
                    .Where(m => m.Member != null)
                    .ToDictionary(m => m.Name, m => m);
        }

        public override IObjectBuilder CreateObject()
        {
            _instanceMemberValues = new Dictionary<string, object>();
            return this;
        }

        public ITypeBuilder GetMemberBuilder(string memberName)
        {
            StructMember member;
            if (_members.TryGetValue(memberName, out member))
            {
                return _registry.GetTypeBuilder(member.Type);
            }
            return _registry.GetTypeBuilder<IJsonObject>();
        }

        public void SetMember(string memberName, object value)
        {
            if (_members.ContainsKey(memberName))
            {
                _instanceMemberValues[memberName] = value;
            }
        }

        public object GetObject()
        {
            var membersToSet = (from kv in _instanceMemberValues
                        select new
                                   {
                                       _members[kv.Key].Member, 
                                       kv.Value
                                   }).ToArray();

            var members = (from v in membersToSet select v.Member).ToArray();
            var values = (from v in membersToSet select v.Value).ToArray();

            return FormatterServices.PopulateObjectMembers(FormatterServices.GetSafeUninitializedObject(typeof(TStruct)), members, values);
        }
    }
}