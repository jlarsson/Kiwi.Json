using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class StructWriter<TStruct> : ITypeWriter where TStruct : struct
    {
        private readonly List<StructMember> _members;
        private readonly ITypeWriterRegistry _registry;

        private StructWriter(ITypeWriterRegistry registry, List<StructMember> members)
        {
            _registry = registry;
            _members = members;
        }

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, object value)
        {
            writer.WriteObjectStart();

            var index = 0;
            foreach (var member in _members)
            {
                if (index++ > 0)
                {
                    writer.WriteObjectMemberDelimiter();
                }
                writer.WriteMember(member.Name);

                var memberValue = member.Getter.GetMemberValue(value);
                var memberWriter = _registry.GetTypeWriterForValue(memberValue);
                memberWriter.Write(writer, memberValue);
            }
            writer.WriteObjectEnd(index);
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory(ITypeWriterRegistry registry)
        {
            var members =
                from field in
                    typeof (TStruct).GetFields(BindingFlags.GetField | BindingFlags.Public |
                                               BindingFlags.Instance)
                select new StructMember
                           {
                               Name = field.Name,
                               Getter = new FieldGetter(field)
                           };

            var writer = new StructWriter<TStruct>(registry, members.ToList());

            return () => writer;
        }

        #region Nested type: StructMember

        private class StructMember
        {
            public string Name { get; set; }
            public IMemberGetter Getter { get; set; }
        }

        #endregion
    }
}