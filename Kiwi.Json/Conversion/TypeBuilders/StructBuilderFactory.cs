using System;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class StructBuilderFactory: ITypeBuilderFactory
    {
        public Func<ITypeBuilder> CreateTypeBuilder(Type type)
        {
            if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
            {
                return
                    (Func<ITypeBuilder>)
                    typeof(StructBuilder<>).MakeGenericType(type).GetMethod("CreateTypeBuilderFactory",
                                                                            BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[]{});
            }
            return null;
        }
    }
}