using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class BuiltinTypeWriterFactory : ITypeWriterFactory
    {
        private static readonly Dictionary<Type, ITypeWriter> BuiltinSerializers =
            new[]
                {
                    CreateSimpleWriter<bool>((w, v) => w.WriteBool(v)),
                    CreateSimpleWriter<sbyte>((w, v) => w.WriteInteger(v)),
                    CreateSimpleWriter<short>((w, v) => w.WriteInteger(v)),
                    CreateSimpleWriter<int>((w, v) => w.WriteInteger(v)),
                    CreateSimpleWriter<long>((w, v) => w.WriteInteger(v)),
                    CreateSimpleWriter<byte>((w, v) => w.WriteInteger(v)),
                    CreateSimpleWriter<ushort>((w, v) => w.WriteInteger(v)),
                    CreateSimpleWriter<uint>((w, v) => w.WriteInteger(v)),
                    CreateSimpleWriter<ulong>((w, v) => w.WriteInteger(Convert.ToInt64(v))),
                    CreateSimpleWriter<double>((w, v) => w.WriteDouble(v)),
                    CreateSimpleWriter<float>((w, v) => w.WriteDouble(v)),
                    CreateSimpleWriter<decimal>((w, v) => w.WriteDouble(Convert.ToDouble(v))),
                    CreateSimpleWriter<DateTime>((w, v) => w.WriteDate(v)),
                    CreateSimpleWriter<Guid>((w, v) => w.WriteString(v.ToString("n"))),
                    CreateSimpleWriter<string>((w, v) => w.WriteString(v)),
                    CreateSimpleWriter<char>((w, v) => w.WriteString(v.ToString(CultureInfo.CurrentCulture))),
                    CreateSimpleNullableWriter<bool>((w, v) => w.WriteBool(v)),
                    CreateSimpleNullableWriter<sbyte>((w, v) => w.WriteInteger(v)),
                    CreateSimpleNullableWriter<short>((w, v) => w.WriteInteger(v)),
                    CreateSimpleNullableWriter<int>((w, v) => w.WriteInteger(v)),
                    CreateSimpleNullableWriter<long>((w, v) => w.WriteInteger(v)),
                    CreateSimpleNullableWriter<byte>((w, v) => w.WriteInteger(v)),
                    CreateSimpleNullableWriter<ushort>((w, v) => w.WriteInteger(v)),
                    CreateSimpleNullableWriter<uint>((w, v) => w.WriteInteger(v)),
                    CreateSimpleNullableWriter<ulong>((w, v) => w.WriteInteger(Convert.ToInt64(v))),
                    CreateSimpleNullableWriter<double>((w, v) => w.WriteDouble(v)),
                    CreateSimpleNullableWriter<float>((w, v) => w.WriteDouble(v)),
                    CreateSimpleNullableWriter<decimal>((w, v) => w.WriteDouble(Convert.ToDouble(v))),
                    CreateSimpleNullableWriter<DateTime>((w, v) => w.WriteDate(v)),
                    CreateSimpleNullableWriter<Guid>((w, v) => w.WriteString(v.ToString("n"))),
                    CreateSimpleNullableWriter<char>((w, v) => w.WriteString(v.ToString(CultureInfo.CurrentCulture)))
                }
                .ToDictionary(t => t.Type, t => t.Writer);

        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            ITypeWriter writer;
            return BuiltinSerializers.TryGetValue(type, out writer) ? writer : null;
        }

        #endregion

        private static WriterInfo CreateSimpleWriter<T>(
            Action<IJsonWriter, T> action)
        {
            return new WriterInfo
                       {
                           Type = typeof (T),
                           Writer = new SimpleWriter<T>(action)
                       };
        }

        private static WriterInfo CreateSimpleNullableWriter<T>(Action<IJsonWriter, T> action)
            where T : struct
        {
            return new WriterInfo
                       {
                           Type = typeof (T?),
                           Writer = new SimpleWriter<T?>(
                               (w, v) =>
                                   {
                                       if (v.HasValue)
                                       {
                                           action(w, v.Value);
                                       }
                                       else
                                       {
                                           w.WriteNull();
                                       }
                                   })
                       };
        }

        #region Nested type: SimpleWriter

        private class SimpleWriter<T> : ITypeWriter
        {
            private readonly Action<IJsonWriter, T> _action;

            public SimpleWriter(Action<IJsonWriter, T> action)
            {
                _action = action;
            }

            #region ITypeWriter Members

            public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
            {
                if (value == null)
                {
                    writer.WriteNull();
                }
                else
                {
                    _action(writer, (T) value);
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: WriterInfo

        private class WriterInfo
        {
            public Type Type { get; set; }
            public ITypeWriter Writer { get; set; }
        }

        #endregion
    }
}