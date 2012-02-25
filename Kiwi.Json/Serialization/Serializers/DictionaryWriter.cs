using System.Collections.Generic;

namespace Kiwi.Json.Serialization.Serializers
{
    public class DictionaryWriter<TValue> : ITypeWriter
    {
        #region ITypeWriter Members

        public void Serialize(ITypeWriterRegistry registry, IJsonWriter writer, object value)
        {
            var dictionary = value as IDictionary<string, TValue>;
            if (dictionary == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteObjectStart();
                int index = 0;
                foreach (var kv in dictionary)
                {
                    if (index++ > 0)
                    {
                        writer.WriteObjectMemberDelimiter();
                    }
                    writer.WriteMember(kv.Key);

                    ITypeWriter memberWriter = registry.GetTypeSerializerForValue(kv.Value);
                    memberWriter.Serialize(registry, writer, kv.Value);
                }
                writer.WriteObjectEnd(index);
            }
        }

        #endregion
    }
}