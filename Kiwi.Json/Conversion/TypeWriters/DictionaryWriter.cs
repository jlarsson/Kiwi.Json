using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class DictionaryWriter<TValue> : ITypeWriter
    {
        private readonly ITypeWriterRegistry _registry;

        private DictionaryWriter(ITypeWriterRegistry registry)
        {
            _registry = registry;
        }

        #region ITypeWriter Members

        public void Serialize(IJsonWriter writer, object value)
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

                    var memberWriter = _registry.GetTypeSerializerForValue(kv.Value);
                    memberWriter.Serialize(writer, kv.Value);
                }
                writer.WriteObjectEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriterRegistry, ITypeWriter> CreateTypeWriterFactory()
        {
            return r => new DictionaryWriter<TValue>(r);
        }
    }
}