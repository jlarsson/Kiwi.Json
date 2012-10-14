using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class EnumBuilder<TEnum> : AbstractTypeBuilder where TEnum : struct
    {
        public static Func<ITypeBuilder> CreateTypeBuilderFactory()
        {
            var builder = new EnumBuilder<TEnum>();
            return () => builder;
        }

        public override object CreateString(ITypeBuilderRegistry registry, string value)
        {
            return Enum.Parse(typeof (TEnum), value);
            // The code below reuires .Net 4
            //
            //if (Enum.TryParse(value, false, out @enum))
            //{
            //    return @enum;
            //}
            //return base.CreateString(registry, value);
        }

        public override object CreateNumber(ITypeBuilderRegistry registry, long value)
        {
            var @enum = Enum.ToObject(typeof (TEnum), value);
            return (TEnum) @enum;
        }

        protected override Type BuildType
        {
            get { return typeof(TEnum); }
        }
    }
}