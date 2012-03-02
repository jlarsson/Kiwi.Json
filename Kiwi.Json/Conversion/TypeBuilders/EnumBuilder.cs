using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class EnumBuilder<TEnum> : AbstractTypeBuilder where TEnum : struct
    {
        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var builder = new EnumBuilder<TEnum>();
            return () => builder;
        }

        public override object CreateString(string value)
        {
            TEnum @enum;
            if (Enum.TryParse(value, false, out @enum))
            {
                return @enum;
            }
            return base.CreateString(value);
        }

        public override object CreateNumber(long value)
        {
            var @enum = Enum.ToObject(typeof (TEnum), value);
            return (TEnum) @enum;
        }
    }
}