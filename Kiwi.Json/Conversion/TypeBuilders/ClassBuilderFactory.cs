using System;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public Func<ITypeBuilder> CreateTypeBuilder(Type type)
        {
            if (type.IsClass)
            {
                return
                    (Func<ITypeBuilder>)
                    typeof (ClassBuilder<>).MakeGenericType(type).GetMethod("CreateTypeBuilderFactory",
                                                                            BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[] {});
            }
            return null;
        }

        #endregion
    }
}