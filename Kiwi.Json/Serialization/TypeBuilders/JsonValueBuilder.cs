using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class JsonValueBuilder : ITypeBuilder
    {
        private static readonly JsonValueBuilder Instance = new JsonValueBuilder();

        #region ITypeBuilder Members

        public IObjectBuilder CreateObject()
        {
            return new JsonObjectBuilder();
        }

        public IArrayBuilder CreateArray()
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

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return _ => Instance;
        }
    }
}