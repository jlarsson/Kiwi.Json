using System;
using System.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public Func<ITypeBuilder> CreateTypeBuilder(Type type, ITypeBuilderRegistry registry)
        {
            if (type.IsClass)
            {
                var constructor = type.GetConstructor(Type.EmptyTypes);

                if (constructor == null)
                {
                    return () => new ClassWithoutDefaultConstructorBuilder(type);
                }

                return
                    (Func<ITypeBuilder>)
                    typeof (ClassBuilder<>).MakeGenericType(type).GetMethod("CreateTypeBuilderFactory",
                                                                            BindingFlags.Static | BindingFlags.Public).
                        Invoke(null, new object[] {registry});
            }
            return null;
        }

        #endregion
    }
}