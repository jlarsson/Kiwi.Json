using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class ClassWriter<TClass> : ITypeWriter where TClass : class
    {
        private static readonly List<ClassMember> Members = DicoverMembers().ToList();

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            var instance = value as TClass;
            if (instance == null)
            {
                writer.WriteNull();
            }
            writer.WriteObjectStart();

            var index = 0;
            foreach (var member in Members)
            {
                if (index++ > 0)
                {
                    writer.WriteObjectMemberDelimiter();
                }
                writer.WriteMember(member.Name);

                var memberValue = member.Getter.GetMemberValue(instance);

                registry.Write(writer, memberValue);
            }
            writer.WriteObjectEnd(index);
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

        private static IEnumerable<ClassMember> DicoverMembers()
        {
            var classUsesDataContract =
                (typeof (TClass).GetCustomAttributes(true).OfType<Attribute>().Where(
                    a => a.GetType().FullName == "System.Runtime.Serialization.DataContractAttribute")).Any();

            return (from property in typeof (TClass).GetProperties(BindingFlags.GetProperty |
                                                                    BindingFlags.Public |
                                                                    BindingFlags.Instance)
                    where (property.GetGetMethod().GetParameters().Length == 0)
                    let dm = classUsesDataContract ? GetDataMemberAttribute(property) : null
                    where classUsesDataContract == (dm != null)
                    select new ClassMember
                                {
                                    Name = GetMemberName(property, dm),
                                    Getter = new PropertyGetter(property),
                                }
                    ).Union(
                        from field in typeof (TClass).GetFields(BindingFlags.GetField |
                                                                BindingFlags.Public |
                                                                BindingFlags.Instance)
                        let dm = classUsesDataContract ? GetDataMemberAttribute(field) : null
                        where classUsesDataContract == (dm != null)
                        select new ClassMember
                                    {
                                        Name = GetMemberName(field, dm),
                                        Getter = new FieldGetter(field),
                                    }
                );
        }

        public static Func<ITypeWriter> CreateTypeWriterFactory()
        {
            return () => new ClassWriter<TClass>();
        }

        #region Nested type: ClassMember

        protected class ClassMember
        {
            public string Name { get; set; }
            public IMemberGetter Getter { get; set; }
        }

        #endregion
    }
}