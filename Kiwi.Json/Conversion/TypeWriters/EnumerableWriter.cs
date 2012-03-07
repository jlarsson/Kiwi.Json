using System;
using System.Collections;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class EnumerableWriter : ITypeWriter
    {
        private readonly ITypeWriterRegistry _registry;

        private EnumerableWriter(ITypeWriterRegistry registry)
        {
            _registry = registry;
        }

        #region ITypeWriter Members

        public void Write(IJsonWriter writer, object value)
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
                    var itemWriter = _registry.GetTypeWriterForValue(item);
                    itemWriter.Write(writer, item);
                }
                writer.WriteArrayEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory(ITypeWriterRegistry registry)
        {
            return () => new EnumerableWriter(registry);
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

        public void Write(IJsonWriter writer, object value)
        {
            var enumerable = value as IEnumerable<T>;
            if (enumerable == null)
            {
                writer.WriteNull();
            }
            else
            {
                var itemWriter = _registry.GetTypeWriterForType(typeof (T));
                writer.WriteArrayStart();

                var index = 0;
                foreach (var item in enumerable)
                {
                    if (index++ > 0)
                    {
                        writer.WriteArrayElementDelimiter();
                    }
                    itemWriter.Write(writer, item);
                }
                writer.WriteArrayEnd(index);
            }
        }

        #endregion

        public static Func<ITypeWriter> CreateTypeWriterFactory(ITypeWriterRegistry registry)
        {
            return () => new EnumerableWriter<T>(registry);
        }
    }
}