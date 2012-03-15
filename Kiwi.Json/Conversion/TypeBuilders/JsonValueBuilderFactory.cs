using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonValueBuilderFactory : ITypeBuilderFactory
    {
        #region ITypeBuilderFactory Members

        public ITypeBuilder CreateTypeBuilder(Type type)
        {
            if (typeof (IJsonValue).IsAssignableFrom(type))
            {
                return new JsonValueBuilder();
            }
            return null;
        }

        #endregion
    }
}