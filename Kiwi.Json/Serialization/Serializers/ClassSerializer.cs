using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Serialization.Serializers
{
    public class ClassSerializer<T> : ITypeSerializer where T : class
    {
        private readonly Dictionary<string, IMemberGetter> _memberGetters;

        public ClassSerializer()
        {
            _memberGetters = (
                                 from property in
                                     typeof (T).GetProperties(BindingFlags.GetProperty |
                                                              BindingFlags.Public |
                                                              BindingFlags.Instance)
                                 where
                                     (property.GetGetMethod().GetParameters().Length == 0)
                                 select new PropertyGetter(property) as IMemberGetter
                             ).Union(
                                 from field in
                                     typeof (T).GetFields(BindingFlags.GetField |
                                                          BindingFlags.Public |
                                                          BindingFlags.Instance)
                                 select new FieldGetter(field) as IMemberGetter
                ).ToDictionary(v => v.MemberName, v => v);
        }

        #region ITypeSerializer Members

        public void Serialize(ITypeSerializerRegistry registry, IJsonWriter writer, object value)
        {
            var instance = value as T;
            if (instance == null)
            {
                writer.WriteNull();
            }
            writer.WriteObjectStart();

            var index = 0;
            foreach (var getter in _memberGetters)
            {
                if (index++ > 0)
                {
                    writer.WriteObjectMemberDelimiter();
                }
                writer.WriteMember(getter.Key);
                var member = getter.Value.GetMemberValue(instance);
                var memberSerializer = registry.GetTypeSerializerForValue(member);
                memberSerializer.Serialize(registry, writer, member);
            }
            writer.WriteObjectEnd();
        }

        #endregion
    }
}