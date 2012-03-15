using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class SystemDataNamespaceTypeFactory : ITypeBuilderFactory
    {
        private static readonly Dictionary<Type, ITypeBuilder> TypeBuilders = new Dictionary<Type, ITypeBuilder>
                                                                                  {
                                                                                  };

        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            ITypeBuilder builder;
            return TypeBuilders.TryGetValue(type, out builder) ? builder : null;
        }

        #endregion
    }
}