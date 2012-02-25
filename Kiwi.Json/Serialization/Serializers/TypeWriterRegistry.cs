using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Untyped;
using Kiwi.Json.Util;

namespace Kiwi.Json.Serialization.Serializers
{
    public class TypeWriterRegistry : ITypeWriterRegistry
    {
        private static readonly Dictionary<Type, Func<ITypeWriterRegistry,ITypeWriter>> BuiltinSerializers =
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

        private static readonly ITypeWriter NullWriter = new SimpleWriter<object>((w, v) => w.WriteNull());

        private readonly IRegistry<Type, Func<ITypeWriterRegistry,ITypeWriter>> _serializers =
            new ThreadsafeRegistry<Type, Func<ITypeWriterRegistry, ITypeWriter>>(BuiltinSerializers);

        #region ITypeWriterRegistry Members

        public ITypeWriter GetTypeSerializerForValue(object value)
        {
            if (value == null)
            {
                return NullWriter;
            }
            return GetTypeSerializerForType(value.GetType());
        }

        public ITypeWriter GetTypeSerializerForType(Type type)
        {
            return _serializers.Lookup(type, CreateTypeSerializerForType)(this);
        }

        #endregion

        private static Tuple<Type, Func<ITypeWriterRegistry,ITypeWriter>> CreateSerializer<T>(Action<IJsonWriter, T> action)
        {
            var writer = new SimpleWriter<T>(action) as ITypeWriter;
            return Tuple.Create(typeof (T), (Func<ITypeWriterRegistry,ITypeWriter>)(_ => writer));
        }

        private Func<ITypeWriterRegistry, ITypeWriter> CreateTypeSerializerForType(Type type)
        {
            if (typeof (IJsonValue).IsAssignableFrom(type))
            {
                return CreateSerializer((IJsonWriter w, IJsonValue v) => v.Write(w)).Item2;
            }

            return (TryCreateSerializerForDictionary(type) ?? TryCreateSerializerForEnumerable(type)) ??
                   TryCreateSerializerForClass(type);
        }

        private Func<ITypeWriterRegistry, ITypeWriter> TryCreateSerializerForClass(Type type)
        {
            if (type.IsClass)
            {
                return (Func<ITypeWriterRegistry, ITypeWriter>)typeof(ClassWriter<>).MakeGenericType(type).GetMethod("CreateTypeWriterFactory", BindingFlags.Static | BindingFlags.Public).
                    Invoke(null, new object[0]);
            }
            return null;
        }

        private Func<ITypeWriterRegistry, ITypeWriter> TryCreateSerializerForEnumerable(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                    let factory = typeof(EnumerableWriter<>).MakeGenericType(@interface.GetGenericArguments()[0])
                    .GetMethod("CreateTypeWriterFactory", BindingFlags.Static | BindingFlags.Public).
                    Invoke(null, new object[0])
                    select (Func<ITypeWriterRegistry, ITypeWriter>)factory
                   )
                .Concat(
                    from @interface in type.GetInterfaces()
                    where @interface == typeof (IEnumerable)
                    select EnumerableWriter.CreateTypeWriterFactory()
                )
                .FirstOrDefault();
        }

        private Func<ITypeWriterRegistry, ITypeWriter> TryCreateSerializerForDictionary(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IDictionary<,>)
                          && @interface.GetGenericArguments()[0] == typeof (string)
                    let factory =
                        typeof (DictionaryWriter<>)
                        .MakeGenericType(@interface.GetGenericArguments()[1])
                        .GetMethod("CreateTypeWriterFactory", BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[0])
                    select (Func<ITypeWriterRegistry, ITypeWriter>) factory
                   )
                .FirstOrDefault();
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

            public void Serialize(IJsonWriter writer, object value)
            {
                _action(writer, (T) value);
            }

            #endregion
        }

        #endregion
    }
}