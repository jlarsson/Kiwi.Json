using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonArrayBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            if (typeof (IJsonArray).IsAssignableFrom(type))
            {
                return new JsonArrayBuilder();
            }
            return null;
        }

        #endregion
    }
}