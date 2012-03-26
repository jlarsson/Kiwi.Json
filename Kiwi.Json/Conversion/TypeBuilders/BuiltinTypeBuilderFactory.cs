using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class BuiltinTypeBuilderFactory : ITypeBuilderFactory
    {
        private static readonly Dictionary<Type, ITypeBuilder> BuiltinTypeBuilders =
            new[]
                {
                    CreateBuilder<bool>(@bool: b => b),
                    CreateBuilder<sbyte>(@long: l => (sbyte) l),
                    CreateBuilder<short>(@long: l => (short) l),
                    CreateBuilder<int>(@long: l => (int) l),
                    CreateBuilder<long>(@long: l => l),
                    CreateBuilder<byte>(@long: l => (byte) l),
                    CreateBuilder<ushort>(@long: l => (ushort) l),
                    CreateBuilder<uint>(@long: l => (uint) l),
                    CreateBuilder<ulong>(@long: l => (ulong) l),
                    CreateBuilder<double>(
                        @long: l => Convert.ToDouble(l),
                        @double: d => d),
                    CreateBuilder<float>(
                        @long: l => Convert.ToSingle(l),
                        @double: d => Convert.ToSingle(d)),
                    CreateBuilder<decimal>(
                        @long: l => Convert.ToDecimal(l),
                        @double: d => Convert.ToDecimal(d)),
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
                        @long: l => Convert.ToDouble(l),
                        @double: d => d),
                    CreateBuilder<float?>(
                        @null: () => null,
                        @long: l => Convert.ToSingle(l),
                        @double: d => Convert.ToSingle(d)),
                    CreateBuilder<decimal?>(
                        @null: () => null,
                        @long: l => Convert.ToDecimal(l),
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
                .ToDictionary(t => t.Type, t => t.Builder);

        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            ITypeBuilder builder;
            return BuiltinTypeBuilders.TryGetValue(type, out builder) ? builder : null;
        }

        #endregion

        private static BuilderInfo CreateBuilder<T>(
            Func<string, object> @string = null,
            Func<long, object> @long = null,
            Func<double, object> @double = null,
            Func<bool, object> @bool = null,
            Func<DateTime, object> @dateTime = null,
            Func<object> @null = null)
        {
            return new BuilderInfo
                       {
                           Type = typeof (T),
                           Builder = new SimpleTypeBuilder
                                         {
                                             String = @string,
                                             Long = @long,
                                             Double = @double,
                                             Bool = @bool,
                                             DateTime = @dateTime,
                                             Null = @null
                                         }
                       };
        }

        #region Nested type: BuilderInfo

        private class BuilderInfo
        {
            public Type Type { get; set; }
            public ITypeBuilder Builder { get; set; }
        }

        #endregion

        #region Nested type: SimpleTypeBuilder

        private class SimpleTypeBuilder : AbstractTypeBuilder
        {
            public Func<string, object> String { private get; set; }
            public Func<long, object> Long { private get; set; }
            public Func<double, object> Double { private get; set; }
            public Func<bool, object> Bool { private get; set; }
            public Func<DateTime, object> DateTime { private get; set; }
            public Func<object> Null { private get; set; }

            public override object CreateNull(ITypeBuilderRegistry registry)
            {
                if (Null != null)
                {
                    return Null();
                }
                return base.CreateNull(registry);
            }

            public override object CreateString(ITypeBuilderRegistry registry, string value)
            {
                if (String != null)
                {
                    return String(value);
                }
                return base.CreateString(registry, value);
            }

            public override object CreateNumber(ITypeBuilderRegistry registry, long value)
            {
                if (Long != null)
                {
                    return Long(value);
                }
                return base.CreateNumber(registry, value);
            }

            public override object CreateNumber(ITypeBuilderRegistry registry, double value)
            {
                if (Double != null)
                {
                    return Double(value);
                }
                return base.CreateNumber(registry, value);
            }

            public override object CreateBool(ITypeBuilderRegistry registry, bool value)
            {
                if (Bool != null)
                {
                    return Bool(value);
                }

                return base.CreateBool(registry, value);
            }

            public override object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue)
            {
                if (DateTime != null)
                {
                    return DateTime(value);
                }
                return base.CreateDateTime(registry, value, sourceValue);
            }
        }

        #endregion
    }
}