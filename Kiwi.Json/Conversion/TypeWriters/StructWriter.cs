using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class StructWriter<TStruct> : ITypeWriter where TStruct : struct
    {
        private static readonly List<StructMember> Members = DiscoverMembers().ToList();

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            writer.WriteObjectStart();

            var index = 0;
            foreach (var member in Members)
            {
                if (index++ > 0)
                {
                    writer.WriteObjectMemberDelimiter();
                }
                writer.WriteMember(member.Name);

                var memberValue = member.Getter.GetMemberValue(value);

                registry.Write(writer, memberValue);
            }
            writer.WriteObjectEnd(index);
        }

        #endregion

        private static IEnumerable<StructMember> DiscoverMembers()
        {
            return
                from field in
                    typeof (TStruct).GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance)
                select new StructMember
                           {
                               Name = field.Name,
                               Getter = new FieldGetter(field)
                           };
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