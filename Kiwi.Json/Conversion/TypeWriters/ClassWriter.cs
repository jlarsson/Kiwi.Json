using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class ClassWriter<T> : ITypeWriter where T : class
    {
        private readonly ITypeWriterRegistry _registry;
        private readonly List<ClassMember> _members;

        protected class ClassMember
        {
            public string Name { get; set; }
            public IMemberGetter Getter { get; set; }
        }

        protected ClassWriter(ITypeWriterRegistry registry, List<ClassMember> members)
        {
            _registry = registry;
            _members = members;
        }

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, object value)
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
                var memberWriter = _registry.GetTypeWriterForValue(memberValue);
                memberWriter.Write(writer, memberValue);
            }
            writer.WriteObjectEnd(index);
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory(ITypeWriterRegistry registry)
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
                              select new ClassMember()
                                         {
                                             Name = field.Name,
                                             Getter = new FieldGetter(field),
                                         }
                );


            return () => new ClassWriter<T>(registry, members.ToList());
        }
    }
}