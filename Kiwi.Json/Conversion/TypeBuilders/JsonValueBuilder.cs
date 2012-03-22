using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonValueBuilder : ITypeBuilder
    {
        #region ITypeBuilder Members

        public virtual IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            return new JsonObjectBuilder();
        }

        public virtual IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
        {
            return new JsonArrayBuilder();
        }

        public virtual object CreateString(ITypeBuilderRegistry registry, string value)
        {
            return new JsonString(value);
        }

        public virtual object CreateNumber(ITypeBuilderRegistry registry, long value)
        {
            return new JsonInteger(value);
        }

        public virtual object CreateNumber(ITypeBuilderRegistry registry, double value)
        {
            return new JsonDouble(value);
        }

        public virtual object CreateBool(ITypeBuilderRegistry registry, bool value)
        {
            return new JsonBool(value);
        }

        public virtual object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue)
        {
            if (sourceValue is string)
            {
                return new JsonDateAndString(value, sourceValue as string);
            }
            return new JsonDate(value);
        }

        public virtual object CreateNull(ITypeBuilderRegistry registry)
        {
            return new JsonNull();
        }

        #endregion
    }
}