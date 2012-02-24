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
    public interface ITypeSerializerRegistry
    {
        ITypeSerializer GetTypeSerializerForValue(object value);
        ITypeSerializer GetTypeSerializerForType(Type type);
    }

    public class TypeSerializerRegistry : ITypeSerializerRegistry
    {
        private static readonly Dictionary<Type, ITypeSerializer> BuiltinSerializers =
            new[]
                {
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
                    CreateSerializer<string>((w, v) => w.WriteString(v)),
                    CreateSerializer<char>((w, v) => w.WriteString(v.ToString(CultureInfo.CurrentCulture)))
                }
                .ToDictionary(t => t.Item1, t => t.Item2);

        private static readonly ITypeSerializer NullSerializer = new SimpleSerializer<object>((w, v) => w.WriteNull());

        private readonly IRegistry<Type, ITypeSerializer> _serializers =
            new ThreadsafeRegistry<Type, ITypeSerializer>(BuiltinSerializers);

        #region ITypeSerializerRegistry Members

        public ITypeSerializer GetTypeSerializerForValue(object value)
        {
            if (value == null)
            {
                return NullSerializer;
            }
            return GetTypeSerializerForType(value.GetType());
        }

        public ITypeSerializer GetTypeSerializerForType(Type type)
        {
            return _serializers.Lookup(type, CreateTypeSerializerForType);
        }

        #endregion

        private static Tuple<Type, ITypeSerializer> CreateSerializer<T>(Action<IJsonWriter, T> action)
        {
            return Tuple.Create(typeof (T), new SimpleSerializer<T>(action) as ITypeSerializer);
        }

        private ITypeSerializer CreateTypeSerializerForType(Type type)
        {
            if (typeof (IJsonValue).IsAssignableFrom(type))
            {
                return CreateSerializer((IJsonWriter w, IJsonValue v) => v.Write(w)).Item2;
            }

            return (TryCreateSerializerForDictionary(type) ?? TryCreateSerializerForEnumerable(type)) ??
                   TryCreateSerializerForClass(type);
        }

        private ITypeSerializer TryCreateSerializerForClass(Type type)
        {
            ConstructorInfo constructorInfo =
                typeof (ClassSerializer<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes);
            if (constructorInfo != null)
            {
                return constructorInfo.Invoke(new object[0]) as ITypeSerializer;
            }
            return null;
        }

        private ITypeSerializer TryCreateSerializerForEnumerable(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                    let constructorInfo = typeof (EnumerableSerializer<>)
                        .MakeGenericType(@interface.GetGenericArguments()[0])
                        .GetConstructor(Type.EmptyTypes)
                    where constructorInfo != null
                    select constructorInfo.Invoke(new object[0]) as ITypeSerializer
                   )
                .Concat(
                    from @interface in type.GetInterfaces()
                    where @interface == typeof (IEnumerable)
                    select new EnumerableSerializer() as ITypeSerializer
                )
                .FirstOrDefault();
        }

        private ITypeSerializer TryCreateSerializerForDictionary(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IDictionary<,>)
                          && @interface.GetGenericArguments()[0] == typeof (string)
                    let constructorInfo = typeof (DictionarySerializer<>)
                        .MakeGenericType(@interface.GetGenericArguments()[1])
                        .GetConstructor(Type.EmptyTypes)
                    where constructorInfo != null
                    select constructorInfo.Invoke(new object[0]) as ITypeSerializer
                   )
                .FirstOrDefault();
        }

        #region Nested type: SimpleSerializer

        private class SimpleSerializer<T> : ITypeSerializer
        {
            private readonly Action<IJsonWriter, T> _action;

            public SimpleSerializer(Action<IJsonWriter, T> action)
            {
                _action = action;
            }

            #region ITypeSerializer Members

            public void Serialize(ITypeSerializerRegistry registry, IJsonWriter writer, object value)
            {
                _action(writer, (T) value);
            }

            #endregion
        }

        #endregion
    }
}