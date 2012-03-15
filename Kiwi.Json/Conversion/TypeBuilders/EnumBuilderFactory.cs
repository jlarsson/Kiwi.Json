using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class EnumBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            if (type.IsEnum)
            {
                return
                    ((ITypeBuilderFactory)
                     typeof (EnumBuilderFactory<>)
                         .MakeGenericType(type)
                         .GetConstructor(Type.EmptyTypes)
                         .Invoke(new object[0])).CreateTypeBuilder(type);
            }
            return null;
        }

        #endregion
    }

    public class EnumBuilderFactory<TEnum> : ITypeBuilderFactory where TEnum : struct
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            return new EnumBuilder<TEnum>();
        }

        #endregion
    }
}