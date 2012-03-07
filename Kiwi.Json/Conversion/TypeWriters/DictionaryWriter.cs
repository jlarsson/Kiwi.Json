using System;
using System.Collections;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class DictionaryWriter<TValue> : ITypeWriter
    {
        private ITypeWriterRegistry _registry;

        private DictionaryWriter(ITypeWriterRegistry registry)
        {
            _registry = registry;
        }

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, object value)
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

                    var valueWriter = _registry.GetTypeWriterForValue(kv.Value);
                    valueWriter.Write(writer, kv.Value);
                }
                writer.WriteObjectEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory(ITypeWriterRegistry registry)
        {
            return () => new DictionaryWriter<TValue>(registry);
        }
    }

    public class DictionaryWriter : ITypeWriter
    {
        private ITypeWriterRegistry _registry;

        private DictionaryWriter(ITypeWriterRegistry registry)
        {
            _registry = registry;
        }

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, object value)
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
                    var valueWriter = _registry.GetTypeWriterForValue(v);
                    valueWriter.Write(writer, v);
                }
                writer.WriteObjectEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory(ITypeWriterRegistry registry)
        {
            return () => new DictionaryWriter(registry);
        }
    }
}