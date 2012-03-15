using System;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class EnumBuilderFactory: ITypeBuilderFactory
    {

        public Func<ITypeBuilder> CreateTypeBuilder(Type type)
        {
            if (type.IsEnum)
            {
                return
                    (Func<ITypeBuilder>)
                    typeof(EnumBuilder<>).MakeGenericType(type).GetMethod("CreateTypeBuilderFactory",
                                                                          BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[]{});
            }
            return null;
        }
    }
}