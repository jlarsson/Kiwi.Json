using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonObjectBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            if (typeof (IJsonObject).IsAssignableFrom(type))
            {
                return new JsonObjectBuilder();
            }
            return null;
        }

        #endregion
    }
}