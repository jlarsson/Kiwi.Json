using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class BuiltinTypeBuilderFactory: ITypeBuilderFactory
    {
        private static readonly Dictionary<Type, Func<ITypeBuilder>> BuiltinTypeBuilders =
            new[]
                {
                    //Tuple.Create(typeof(object), SystemObjectBuilder.CreateTypeBuilderFactory()),

                    //Tuple.Create(typeof (IJsonValue), JsonValueBuilder.CreateTypeBuilderFactory()),
                    //Tuple.Create(typeof (IJsonArray), JsonArrayBuilder.CreateTypeBuilderFactory()),
                    //Tuple.Create(typeof (JsonArray), JsonArrayBuilder.CreateTypeBuilderFactory()),
                    //Tuple.Create(typeof (IJsonObject), JsonObjectBuilder.CreateTypeBuilderFactory()),
                    //Tuple.Create(typeof (JsonObject), JsonObjectBuilder.CreateTypeBuilderFactory()),
                    CreateBuilder<bool>(@bool: b => b),
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
                    CreateBuilder<Guid>(@string: s => new Guid(s)),
                    CreateBuilder<string>(
                        @null: () => null,
                        @string: s => s,
                        @long: l => l.ToString(CultureInfo.CurrentCulture),
                        @double: d => d.ToString(CultureInfo.CurrentCulture),
                        @bool: b => b.ToString(CultureInfo.CurrentCulture),
                        @dateTime: d => d.ToString(CultureInfo.CurrentCulture)
                        ),
                    CreateBuilder<char>(@string: s => s.FirstOrDefault()),
                    CreateBuilder<bool?>(
                        @null: () => null,
                        @bool: b => b),
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
                    CreateBuilder<Guid?>(
                        @null: () => null,
                        @string: s => new Guid(s)),
                    CreateBuilder<char?>(
                        @null: () => null,
                        @string: s => s.FirstOrDefault()
                        )
                }
                .ToDictionary(t => t.Item1, t => t.Item2);

        private static Tuple<Type, Func<ITypeBuilder>> CreateBuilder<T>(
            Func<string, object> @string = null,
            Func<long, object> @long = null,
            Func<double, object> @double = null,
            Func<bool, object> @bool = null,
            Func<DateTime, object> @dateTime = null,
            Func<object> @null = null)
        {
            var builder = new SimpleTypeBuilder
            {
                String = @string,
                Long = @long,
                Double = @double,
                Bool = @bool,
                DateTime = @dateTime,
                Null = @null
            };
            return Tuple.Create(typeof(T), (Func<ITypeBuilder>)(() => builder));
        }

        private class SimpleTypeBuilder : AbstractTypeBuilder
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

            public override object CreateDateTime(DateTime value, object sourceValue)
            {
                if (DateTime != null)
                {
                    return DateTime(value);
                }
                return base.CreateDateTime(value, sourceValue);
            }
        }

        public Func<ITypeBuilder> CreateTypeBuilder(Type type, ITypeBuilderRegistry registry)
        {
            var factory = default(Func<ITypeBuilder>);
            return BuiltinTypeBuilders.TryGetValue(type, out factory) ? factory : null;
        }
    }
}