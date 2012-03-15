using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ArrayBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            if (type.IsArray && type.GetArrayRank() == 1)
            {
                var elementType = type.GetElementType();

                return
                    ((ITypeBuilderFactory)
                     typeof (ArrayBuilderFactory<>)
                         .MakeGenericType(elementType)
                         .GetConstructor(Type.EmptyTypes)
                         .Invoke(new object[0])).CreateTypeBuilder(type);
            }
            return null;
        }

        #endregion
    }

    public class ArrayBuilderFactory<TElem> : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            return new ArrayBuilder<TElem>();
        }

        #endregion
    }
}