using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class SystemObjectBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            if (type == typeof (object))
            {
                return new SystemObjectBuilder();
            }
            return null;
        }

        #endregion
    }
}