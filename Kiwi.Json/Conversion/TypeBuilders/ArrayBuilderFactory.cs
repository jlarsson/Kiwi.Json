using System;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ArrayBuilderFactory: ITypeBuilderFactory
    {
        public Func<ITypeBuilder> CreateTypeBuilder(Type type, ITypeBuilderRegistry registry)
        {
            if (type.IsArray && type.GetArrayRank() == 1)
            {
                var elementType = type.GetElementType();

                return
                    (Func<ITypeBuilder>)
                    typeof(ArrayBuilder<>).MakeGenericType(elementType)
                        .GetMethod("CreateTypeBuilderFactory",BindingFlags.Static |BindingFlags.Public)
                        .Invoke(null, new object[]{registry});
            }
            return null;
        }
    }
}