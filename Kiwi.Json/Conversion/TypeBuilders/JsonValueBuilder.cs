using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonValueBuilder : ITypeBuilder
    {
        #region ITypeBuilder Members

        public IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            return new JsonObjectBuilder();
        }

        public IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
        {
            return new JsonArrayBuilder();
        }

        public object CreateString(ITypeBuilderRegistry registry, string value)
        {
            return new JsonString(value);
        }

        public object CreateNumber(ITypeBuilderRegistry registry, long value)
        {
            return new JsonInteger(value);
        }

        public object CreateNumber(ITypeBuilderRegistry registry, double value)
        {
            return new JsonDouble(value);
        }

        public object CreateBool(ITypeBuilderRegistry registry, bool value)
        {
            return new JsonBool(value);
        }

        public object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue)
        {
            if (sourceValue is string)
            {
                return new JsonDateAndString(value, sourceValue as string);
            }
            return new JsonDate(value);
        }

        public object CreateNull(ITypeBuilderRegistry registry)
        {
            return new JsonNull();
        }

        #endregion
    }
}