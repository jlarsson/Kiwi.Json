using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class ClassWriter<T> : ITypeWriter where T : class
    {
        private readonly List<ClassMember> _members;

        protected ClassWriter(List<ClassMember> members)
        {
            _members = members;
        }

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            var instance = value as T;
            if (instance == null)
            {
                writer.WriteNull();
            }
            writer.WriteObjectStart();

            var index = 0;
            foreach (var member in _members)
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

        public static Func<ITypeWriter> CreateTypeWriterFactory()
        {
            var members = (
                              from property in
                                  typeof (T).GetProperties(BindingFlags.GetProperty |
                                                           BindingFlags.Public |
                                                           BindingFlags.Instance)
                              where
                                  (property.GetGetMethod().GetParameters().Length == 0)
                              select new ClassMember
                                         {
                                             Name = property.Name,
                                             Getter = new PropertyGetter(property),
                                         }
                          ).Union(
                              from field in
                                  typeof (T).GetFields(BindingFlags.GetField |
                                                       BindingFlags.Public |
                                                       BindingFlags.Instance)
                              select new ClassMember
                                         {
                                             Name = field.Name,
                                             Getter = new FieldGetter(field),
                                         }
                );


            return () => new ClassWriter<T>(members.ToList());
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