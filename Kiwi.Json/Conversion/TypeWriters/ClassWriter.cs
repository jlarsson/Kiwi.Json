using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class ClassWriter<T> : ITypeWriter where T : class
    {
        private readonly Dictionary<string, IMemberGetter> _memberGetters;
        private readonly ITypeWriterRegistry _registry;

        public ClassWriter(ITypeWriterRegistry registry, Dictionary<string, IMemberGetter> memberGetters)
        {
            _registry = registry;
            _memberGetters = memberGetters;
        }

        #region ITypeWriter Members

        public void Serialize(IJsonWriter writer, object value)
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
                var memberWriter = _registry.GetTypeSerializerForValue(member);
                memberWriter.Serialize(writer, member);
            }
            writer.WriteObjectEnd(index);
        }

        #endregion

        public static Func<ITypeWriterRegistry, ITypeWriter> CreateTypeWriterFactory()
        {
            var memberGetters = (
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


            return r => new ClassWriter<T>(r, memberGetters);
        }
    }
}