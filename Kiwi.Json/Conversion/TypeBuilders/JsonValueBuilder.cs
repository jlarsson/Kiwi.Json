using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonValueBuilder : ITypeBuilder
    {
        #region ITypeBuilder Members

        public IObjectBuilder CreateObjectBuilder()
        {
            return new JsonObjectBuilder();
        }

        public IArrayBuilder CreateArrayBuilder()
        {
            return new JsonArrayBuilder();
        }

        public object CreateString(string value)
        {
            return new JsonString(value);
        }

        public object CreateNumber(long value)
        {
            return new JsonInteger(value);
        }

        public object CreateNumber(double value)
        {
            return new JsonDouble(value);
        }

        public object CreateBool(bool value)
        {
            return new JsonBool(value);
        }

        public object CreateDateTime(DateTime value)
        {
            return new JsonDate(value);
        }

        public object CreateNull()
        {
            return new JsonNull();
        }

        #endregion
    }
}