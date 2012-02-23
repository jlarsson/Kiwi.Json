using System.Collections.Generic;

namespace Kiwi.Json.Serialization.Serializers
{
    public class DictionarySerializer<TValue> : ITypeSerializer
    {
        #region ITypeSerializer Members

        public void Serialize(ITypeSerializerRegistry registry, IJsonWriter writer, object value)
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

                    var memberSerializer = registry.GetTypeSerializerForValue(kv.Value);
                    memberSerializer.Serialize(registry, writer, kv.Value);
                }
                writer.WriteObjectEnd();
            }
        }

        #endregion
    }
}