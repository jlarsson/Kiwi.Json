using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            if (type.IsClass && !typeof(Delegate).IsAssignableFrom(type))
            {
                return
                    ((ITypeBuilderFactory)
                     typeof (ClassBuilderFactory<>)
                         .MakeGenericType(type)
                         .GetConstructor(Type.EmptyTypes)
                         .Invoke(new object[0])).CreateTypeBuilder(type);
            }
            return null;
        }

        #endregion
    }

    public class ClassBuilderFactory<TClass> : ITypeBuilderFactory where TClass : class
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            return new ClassBuilder<TClass>();
        }

        #endregion
    }
}