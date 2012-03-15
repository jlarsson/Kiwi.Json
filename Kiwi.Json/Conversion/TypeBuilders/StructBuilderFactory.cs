using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class StructBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
            {
                return
                    ((ITypeBuilderFactory)
                     typeof (StructBuilderFactory<>)
                         .MakeGenericType(type)
                         .GetConstructor(Type.EmptyTypes)
                         .Invoke(new object[0])).CreateTypeBuilder(type);
            }
            return null;
        }

        #endregion
    }

    public class StructBuilderFactory<TStruct> : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            return new StructBuilder<TStruct>();
        }

        #endregion
    }
}