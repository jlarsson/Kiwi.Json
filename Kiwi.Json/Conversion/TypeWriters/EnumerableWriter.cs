using System;
using System.Collections;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumerableWriter : ITypeWriter
    {
        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
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
                    registry.Write(writer, item);
                }
                writer.WriteArrayEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory()
        {
            return () => new EnumerableWriter();
        }
    }

    public class EnumerableWriter<T> : ITypeWriter
    {
        #region ITypeWriter Members

        public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
        {
            var enumerable = value as IEnumerable<T>;
            if (enumerable == null)
            {
                writer.WriteNull();
            }
            else
            {
                var itemWriter = registry.GetTypeWriterForType(typeof (T));
                writer.WriteArrayStart();

                var index = 0;
                foreach (var item in enumerable)
                {
                    if (index++ > 0)
                    {
                        writer.WriteArrayElementDelimiter();
                    }
                    itemWriter.Write(writer, registry, item);
                }
                writer.WriteArrayEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory()
        {
            return () => new EnumerableWriter<T>();
        }
    }
}