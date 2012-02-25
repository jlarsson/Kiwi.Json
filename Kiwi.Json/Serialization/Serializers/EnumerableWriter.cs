using System;
using System.Collections;
using System.Collections.Generic;

namespace Kiwi.Json.Serialization.Serializers
{
    public class EnumerableWriter : ITypeWriter
    {
        private readonly ITypeWriterRegistry _registry;

        private EnumerableWriter(ITypeWriterRegistry registry)
        {
            _registry = registry;
        }

        #region ITypeWriter Members

        public void Serialize(IJsonWriter writer, object value)
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
                    ITypeWriter itemWriter = _registry.GetTypeSerializerForValue(item);
                    itemWriter.Serialize(writer, item);
                }
                writer.WriteArrayEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriterRegistry, ITypeWriter> CreateTypeWriterFactory()
        {
            return r => new EnumerableWriter(r);
        }
    }

    public class EnumerableWriter<T> : ITypeWriter
    {
        private readonly ITypeWriterRegistry _registry;

        private EnumerableWriter(ITypeWriterRegistry registry)
        {
            _registry = registry;
        }

        #region ITypeWriter Members

        public void Serialize(IJsonWriter writer, object value)
        {
            var enumerable = value as IEnumerable<T>;
            if (enumerable == null)
            {
                writer.WriteNull();
            }
            else
            {
                ITypeWriter itemWriter = _registry.GetTypeSerializerForType(typeof (T));
                writer.WriteArrayStart();

                int index = 0;
                foreach (T item in enumerable)
                {
                    if (index++ > 0)
                    {
                        writer.WriteArrayElementDelimiter();
                    }
                    itemWriter.Serialize(writer, item);
                }
                writer.WriteArrayEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriterRegistry, ITypeWriter> CreateTypeWriterFactory()
        {
            return r => new EnumerableWriter<T>(r);
        }
    }
}