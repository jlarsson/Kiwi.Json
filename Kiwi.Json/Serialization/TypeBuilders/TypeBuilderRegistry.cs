using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Kiwi.Json.Conversion.Reflection;
using Kiwi.Json.Serialization.Serializers;
using Kiwi.Json.Untyped;
using Kiwi.Json.Util;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class TypeBuilderRegistry : ITypeBuilderRegistry
    {
        private class SimpleTypeBuilder: AbstractTypeBuilder
        {
            public Func<string, object> String { private get; set; }
            public Func<long, object> Long { private get; set; }
            public Func<double, object> Double { private get; set; }
            public Func<bool, object> Bool { private get; set; }
            public Func<DateTime, object> DateTime { private get; set; }
            public Func<object> Null { private get; set; }

            public override object CreateNull()
            {
                if (Null != null)
                {
                    return Null();
                }
                return base.CreateNull();
            }

            public override object CreateString(string value)
            {
                if (String != null)
                {
                    return String(value);
                }
                return base.CreateString(value);
            }

            public override object CreateNumber(long value)
            {
                if (Long != null)
                {
                    return Long(value);
                }
                return base.CreateNumber(value);
            }

            public override object CreateNumber(double value)
            {
                if (Double != null)
                {
                    return Double(value);
                }
                return base.CreateNumber(value);
            }

            public override object CreateBool(bool value)
            {
                if (Bool != null)
                {
                    return Bool(value);
                }

                return base.CreateBool(value);
            }

            public override object CreateDateTime(DateTime value)
            {
                if (DateTime != null)
                {
                    return DateTime(value);
                }
                return base.CreateDateTime(value);
            }
        }

        private static readonly Dictionary<Type, ITypeBuilder> BuiltinTypeBuilders =
            new[]
                {
                    Tuple.Create<Type,ITypeBuilder>(typeof(IJsonValue), new JsonValueBuilder()),

                    Tuple.Create<Type,ITypeBuilder>(typeof(IJsonArray), new JsonArrayBuilder()),
                    Tuple.Create<Type,ITypeBuilder>(typeof(JsonArray), new JsonArrayBuilder()),

                    Tuple.Create<Type,ITypeBuilder>(typeof(IJsonObject), new JsonObjectBuilder()),
                    Tuple.Create<Type,ITypeBuilder>(typeof(JsonObject), new JsonObjectBuilder()),


                    CreateBuilder<sbyte>(@long: l => (sbyte) l),
                    CreateBuilder<short>(@long: l => (short) l),
                    CreateBuilder<int>(@long: l => (int) l),
                    CreateBuilder<long>(@long: l => l),
                    CreateBuilder<byte>(@long: l => (byte) l),
                    CreateBuilder<ushort>(@long: l => (ushort) l),
                    CreateBuilder<uint>(@long: l => (uint) l),
                    CreateBuilder<ulong>(@long: l => (ulong) l),
                    CreateBuilder<double>(@double: d => d),
                    CreateBuilder<float>(@double: d => Convert.ToSingle(d)),
                    CreateBuilder<decimal>(@double: d => Convert.ToDecimal(d)),

                    CreateBuilder<DateTime>(@dateTime: d => d),
                    CreateBuilder<string>(
                        @null: () => null,
                        @string: s => s,
                        @long: l => l.ToString(CultureInfo.CurrentCulture),
                        @double: d => d.ToString(CultureInfo.CurrentCulture),
                        @bool: b => b.ToString(CultureInfo.CurrentCulture),
                        @dateTime: d => d.ToString(CultureInfo.CurrentCulture)
                        ),

                    CreateBuilder<sbyte?>(
                        @null: () => null,
                        @long: l => (sbyte) l),
                    CreateBuilder<short?>(
                        @null: () => null,
                        @long: l => (short) l),
                    CreateBuilder<int?>(
                        @null: () => null,
                        @long: l => (int) l),
                    CreateBuilder<long?>(
                        @null: () => null,
                        @long: l => l),
                    CreateBuilder<byte?>(
                        @null: () => null,
                        @long: l => (byte) l),
                    CreateBuilder<ushort?>(
                        @null: () => null,
                        @long: l => (ushort) l),
                    CreateBuilder<uint?>(
                        @null: () => null,
                        @long: l => (uint) l),
                    CreateBuilder<ulong?>(
                        @null: () => null,
                        @long: l => (ulong) l),
                    CreateBuilder<double?>(
                        @null: () => null,
                        @double: d => d),
                    CreateBuilder<float?>(
                        @null: () => null,
                        @double: d => Convert.ToSingle(d)),
                    CreateBuilder<decimal?>(
                        @null: () => null,
                        @double: d => Convert.ToDecimal(d)),
                    CreateBuilder<DateTime?>(
                        @null: () => null,
                        @dateTime: d => d),

                }
                .ToDictionary(t => t.Item1, t => t.Item2);

        //private readonly IRegistry<Type, ITypeBuilder> _typeBuilders = new ThreadsafeRegistry<Type, ITypeBuilder>(BuiltinTypeBuilders);
        private readonly IRegistry<Type, ITypeBuilder> _typeBuilders = new ThreadsafeRegistry<Type, ITypeBuilder>();

        #region ITypeBuilderRegistry Members

        public ITypeBuilder GetTypeBuilder<T>()
        {
            return GetTypeBuilder(typeof (T));
        }

        public ITypeBuilder GetTypeBuilder(Type type)
        {
            return _typeBuilders.Lookup(type, CreateTypeBuilder);
        }

        private ITypeBuilder CreateTypeBuilder(Type type)
        {
            return TryGetBuiltinTypeBuilder(type)
                   ?? TryCreateTypeBuilderForArray(type)
                   ?? TryCreateTypeBuilderForDictionary(type)
                   ?? TryCreateTypeBuilderForEnumerable(type)
                   ?? TryCreateTypeBuilderForClass(type)
                   ?? TryCreateTypeBuilderForEnum(type)
                   ?? TryCreateTypeBuilderForStruct(type);
                ;
        }

        private ITypeBuilder TryCreateTypeBuilderForStruct(Type type)
        {
            if (type.IsValueType &&  !type.IsPrimitive && !type.IsEnum)
            {
                var typeBuilderConstructor = typeof(StructBuilder<>).MakeGenericType(type).GetConstructor(new[] { typeof(ITypeBuilderRegistry) });
                if (typeBuilderConstructor != null)
                {
                    return typeBuilderConstructor.Invoke(new object[] { this }) as ITypeBuilder;
                }
            }
            return null;
        }

        private ITypeBuilder TryGetBuiltinTypeBuilder(Type type)
        {
            var typeBuilder = default(ITypeBuilder);
            return BuiltinTypeBuilders.TryGetValue(type, out typeBuilder) ? typeBuilder : null;
        }

        private ITypeBuilder TryCreateTypeBuilderForArray(Type type)
        {
            if (type.IsArray && type.GetArrayRank() == 1)
            {
                var elementType = type.GetElementType();
                var constructorInfo = typeof (ArrayBuilder<>).MakeGenericType(elementType).GetConstructor(new[] {typeof (ITypeBuilderRegistry)});
                if (constructorInfo != null)
                {
                    return constructorInfo
                               .Invoke(new object[] {this}) as ITypeBuilder;
                }
            }
            return null;
        }

        private ITypeBuilder TryCreateTypeBuilderForClass(Type type)
        {
            if (type.IsClass)
            {
                var constructor = type.GetConstructor(Type.EmptyTypes);

                if (constructor == null)
                {
                    return new InvalidClassBuilder(type);
                }

                var typeBuilderConstructor = typeof(ClassBuilder<>).MakeGenericType(type).GetConstructor(new[] { typeof(ITypeBuilderRegistry) });
                if (typeBuilderConstructor != null)
                {
                    return typeBuilderConstructor.Invoke(new object[] {this}) as ITypeBuilder;
                }
            }
            return null;
        }

        private ITypeBuilder TryCreateTypeBuilderForEnumerable(Type type)
        {
            // Check which IList<T> is implemented
            var interfaceType = (from @interface in new[] {type}.Concat(type.GetInterfaces())
                                 where
                                     @interface.IsGenericType &&
                                     @interface.GetGenericTypeDefinition() == typeof (IList<>)
                                 select @interface)
                .FirstOrDefault();

            if (interfaceType == null)
            {
                // Check if it is IEnumerable<T>
                interfaceType = (from @interface in new[] {type}.Concat(type.GetInterfaces())
                                 where
                                     @interface.IsGenericType &&
                                     @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                                 select @interface)
                    .FirstOrDefault();
            }
            if (interfaceType == null)
            {
                return null;
            }

            var elementType = interfaceType.GetGenericArguments()[0];

            // Determine concrete IList<T> to instantiate
            var listType = type.IsInterface
                               ? typeof (List<>).MakeGenericType(elementType)
                               : type;

            if (!type.IsAssignableFrom(listType))
            {
                return null;
            }

            // List must have default constructor
            if (listType.GetConstructor(Type.EmptyTypes) == null)
            {
                return null;
            }


            var constructorInfo = typeof (ListBuilder<,>).MakeGenericType(listType, interfaceType.GetGenericArguments()[0]).GetConstructor(new[] {typeof (ITypeBuilderRegistry)});
            if (constructorInfo != null)
            {
                return constructorInfo
                           .Invoke(new object[] {this}) as ITypeBuilder;
            }

            return null;
        }

        private ITypeBuilder TryCreateTypeBuilderForDictionary(Type type)
        {
            return null;
        }

        private ITypeBuilder TryCreateTypeBuilderForEnum(Type type)
        {
            if (type.IsEnum)
            {
                var constructorInfo = typeof (EnumBuilder<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes);
                if (constructorInfo != null)
                {
                    return constructorInfo.Invoke(new object[0]) as ITypeBuilder;
                }
            }
            return null;
        }

        #endregion

        private static Tuple<Type, ITypeBuilder> CreateBuilder<T>(
            Func<string, object> @string = null,
            Func<long, object> @long = null,
            Func<double, object> @double = null,
            Func<bool, object> @bool = null,
            Func<DateTime, object> @dateTime = null,
            Func<object> @null = null)
        {
            return Tuple.Create(typeof (T), new SimpleTypeBuilder
                                                {
                                                    String = @string,
                                                    Long = @long,
                                                    Double = @double,
                                                    Bool = @bool,
                                                    DateTime = @dateTime,
                                                    Null = @null
                                                } as ITypeBuilder);
        }
    }
}