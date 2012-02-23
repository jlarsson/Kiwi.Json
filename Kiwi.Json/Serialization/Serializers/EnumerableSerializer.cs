using System.Collections;
using System.Collections.Generic;

namespace Kiwi.Json.Serialization.Serializers
{
    public class EnumerableSerializer : ITypeSerializer
    {
        #region ITypeSerializer Members

        public void Serialize(ITypeSerializerRegistry registry, IJsonWriter writer, object value)
        {
            var enumerable = value as IEnumerable;
            if (enumerable == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteArrayStart();

                var index = 0;
                foreach (var item in enumerable)
                {
                    if (index++ > 0)
                    {
                        writer.WriteArrayElementDelimiter();
                    }
                    var itemSerializer = registry.GetTypeSerializerForValue(item);
                    itemSerializer.Serialize(registry, writer, item);
                }
                writer.WriteArrayEnd();
            }
        }

        #endregion
    }

    public class EnumerableSerializer<T> : ITypeSerializer
    {
        #region ITypeSerializer Members

        public void Serialize(ITypeSerializerRegistry registry, IJsonWriter writer, object value)
        {
            var enumerable = value as IEnumerable<T>;
            if (enumerable == null)
            {
                writer.WriteNull();
            }
            else
            {
                var itemSerializer = registry.GetTypeSerializerForType(typeof (T));
                writer.WriteArrayStart();

                var index = 0;
                foreach (var item in enumerable)
                {
                    if (index++ > 0)
                    {
                        writer.WriteArrayElementDelimiter();
                    }
                    itemSerializer.Serialize(registry, writer, item);
                }
                writer.WriteArrayEnd();
            }
        }

        #endregion
    }
}