using System.Collections;
using System.Collections.Generic;

namespace Kiwi.Json.Serialization.Serializers
{
    public class EnumerableWriter : ITypeWriter
    {
        #region ITypeWriter Members

        public void Serialize(ITypeWriterRegistry registry, IJsonWriter writer, object value)
        {
            var enumerable = value as IEnumerable;
            if (enumerable == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteArrayStart();

                int index = 0;
                foreach (object item in enumerable)
                {
                    if (index++ > 0)
                    {
                        writer.WriteArrayElementDelimiter();
                    }
                    ITypeWriter itemWriter = registry.GetTypeSerializerForValue(item);
                    itemWriter.Serialize(registry, writer, item);
                }
                writer.WriteArrayEnd(index);
            }
        }

        #endregion
    }

    public class EnumerableWriter<T> : ITypeWriter
    {
        #region ITypeWriter Members

        public void Serialize(ITypeWriterRegistry registry, IJsonWriter writer, object value)
        {
            var enumerable = value as IEnumerable<T>;
            if (enumerable == null)
            {
                writer.WriteNull();
            }
            else
            {
                ITypeWriter itemWriter = registry.GetTypeSerializerForType(typeof (T));
                writer.WriteArrayStart();

                int index = 0;
                foreach (T item in enumerable)
                {
                    if (index++ > 0)
                    {
                        writer.WriteArrayElementDelimiter();
                    }
                    itemWriter.Serialize(registry, writer, item);
                }
                writer.WriteArrayEnd(index);
            }
        }

        #endregion
    }
}