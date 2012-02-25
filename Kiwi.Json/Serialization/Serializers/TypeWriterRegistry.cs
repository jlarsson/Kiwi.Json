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
        private static readonly Dictionary<Type, ITypeWriter> BuiltinSerializers =
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

        private readonly IRegistry<Type, ITypeWriter> _serializers =
            new ThreadsafeRegistry<Type, ITypeWriter>(BuiltinSerializers);

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
            return _serializers.Lookup(type, CreateTypeSerializerForType);
        }

        #endregion

        private static Tuple<Type, ITypeWriter> CreateSerializer<T>(Action<IJsonWriter, T> action)
        {
            return Tuple.Create(typeof (T), new SimpleWriter<T>(action) as ITypeWriter);
        }

        private ITypeWriter CreateTypeSerializerForType(Type type)
        {
            if (typeof (IJsonValue).IsAssignableFrom(type))
            {
                return CreateSerializer((IJsonWriter w, IJsonValue v) => v.Write(w)).Item2;
            }

            return (TryCreateSerializerForDictionary(type) ?? TryCreateSerializerForEnumerable(type)) ??
                   TryCreateSerializerForClass(type);
        }

        private ITypeWriter TryCreateSerializerForClass(Type type)
        {
            ConstructorInfo constructorInfo =
                typeof (ClassWriter<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes);
            if (constructorInfo != null)
            {
                return constructorInfo.Invoke(new object[0]) as ITypeWriter;
            }
            return null;
        }

        private ITypeWriter TryCreateSerializerForEnumerable(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                    let constructorInfo = typeof (EnumerableWriter<>)
                        .MakeGenericType(@interface.GetGenericArguments()[0])
                        .GetConstructor(Type.EmptyTypes)
                    where constructorInfo != null
                    select constructorInfo.Invoke(new object[0]) as ITypeWriter
                   )
                .Concat(
                    from @interface in type.GetInterfaces()
                    where @interface == typeof (IEnumerable)
                    select new EnumerableWriter() as ITypeWriter
                )
                .FirstOrDefault();
        }

        private ITypeWriter TryCreateSerializerForDictionary(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IDictionary<,>)
                          && @interface.GetGenericArguments()[0] == typeof (string)
                    let constructorInfo = typeof (DictionaryWriter<>)
                        .MakeGenericType(@interface.GetGenericArguments()[1])
                        .GetConstructor(Type.EmptyTypes)
                    where constructorInfo != null
                    select constructorInfo.Invoke(new object[0]) as ITypeWriter
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

            public void Serialize(ITypeWriterRegistry registry, IJsonWriter writer, object value)
            {
                _action(writer, (T) value);
            }

            #endregion
        }

        #endregion
    }
}