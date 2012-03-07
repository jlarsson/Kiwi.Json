using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class BuiltinTypeWriterFactory: ITypeWriterFactory
    {
        private static readonly Dictionary<Type, Func<ITypeWriter>> BuiltinSerializers =
            new[]
                {
                    CreateSerializer<bool>((w, v) => w.WriteBool(v)),
                    CreateSerializer<sbyte>((w, v) => w.WriteInteger(v)),
                    CreateSerializer<short>((w, v) => w.WriteInteger(v)),
                    CreateSerializer<int>((w, v) => w.WriteInteger(v)),
                    CreateSerializer<long>((w, v) => w.WriteInteger(v)),
                    CreateSerializer<byte>((w, v) => w.WriteInteger(v)),
                    CreateSerializer<ushort>((w, v) => w.WriteInteger(v)),
                    CreateSerializer<uint>((w, v) => w.WriteInteger(v)),
                    CreateSerializer<ulong>((w, v) => w.WriteInteger(Convert.ToInt64(v))),
                    CreateSerializer<double>((w, v) => w.WriteDouble(v)),
                    CreateSerializer<float>((w, v) => w.WriteDouble(v)),
                    CreateSerializer<decimal>((w, v) => w.WriteDouble(Convert.ToDouble(v))),
                    CreateSerializer<DateTime>((w, v) => w.WriteDate(v)),
                    CreateSerializer<Guid>((w, v) => w.WriteString(v.ToString("n"))),
                    CreateSerializer<string>((w, v) => w.WriteString(v)),
                    CreateSerializer<char>((w, v) => w.WriteString(v.ToString(CultureInfo.CurrentCulture)))
                }
                .ToDictionary(t => t.Item1, t => t.Item2);

        public Func<ITypeWriter> CreateTypeWriter(Type type, ITypeWriterRegistry registry)
        {
            Func<ITypeWriter> factory;
            return BuiltinSerializers.TryGetValue(type, out factory) ? factory : null;
        }

        private static Tuple<Type, Func<ITypeWriter>> CreateSerializer<T>(
            Action<IJsonWriter, T> action)
        {
            var writer = new SimpleWriter<T>(action) as ITypeWriter;
            return Tuple.Create(typeof(T), (Func<ITypeWriter>)(() => writer));
        }

        private class SimpleWriter<T> : ITypeWriter
        {
            private readonly Action<IJsonWriter, T> _action;

            public SimpleWriter(Action<IJsonWriter, T> action)
            {
                _action = action;
            }

            #region ITypeWriter Members

            public void Write(IJsonWriter writer, object value)
            {
                if (value == null)
                {
                    writer.WriteNull();
                }
                else
                {
                    _action(writer, (T)value);
                }
            }

            #endregion
        }
    }
}