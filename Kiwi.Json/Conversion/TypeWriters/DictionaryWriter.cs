using System;
using System.Collections;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class DictionaryWriter<TValue> : ITypeWriter
    {
        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            var dictionary = value as IDictionary<string, TValue>;
            if (dictionary == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteObjectStart();
                var index = 0;
                foreach (var kv in dictionary)
                {
                    if (index++ > 0)
                    {
                        writer.WriteObjectMemberDelimiter();
                    }
                    writer.WriteMember(kv.Key);

                    registry.Write(writer, kv.Value);
                }
                writer.WriteObjectEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory()
        {
            return () => new DictionaryWriter<TValue>();
        }
    }

    public class DictionaryWriter : ITypeWriter
    {
        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            var dictionary = value as IDictionary;
            if (dictionary == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteObjectStart();
                var index = 0;

                foreach (var key in dictionary.Keys)
                {
                    if (index++ > 0)
                    {
                        writer.WriteObjectMemberDelimiter();
                    }
                    writer.WriteMember(key == null ? "" : key.ToString());

                    var v = dictionary[key];

                    registry.Write(writer, v);
                }
                writer.WriteObjectEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory()
        {
            return () => new DictionaryWriter();
        }
    }
}