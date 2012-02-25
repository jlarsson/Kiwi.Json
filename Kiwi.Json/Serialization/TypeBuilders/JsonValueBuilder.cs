using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class JsonValueBuilder : ITypeBuilder
    {
        private static readonly JsonValueBuilder Instance = new JsonValueBuilder();
        private static readonly JsonValueBuilder ToSystemObjectInstance = new JsonValueBuilder()
                                                                              {
                                                                                  ConvertResultObject = v => v.ToObject()
                                                                              };

        public Func<IJsonValue, object> ConvertResultObject { get; set; }

        public JsonValueBuilder()
        {
            ConvertResultObject = _ => _;
        }

        #region ITypeBuilder Members

        public IObjectBuilder CreateObject()
        {
            return new JsonObjectBuilder(){ConvertResultObject = ConvertResultObject};
        }

        public IArrayBuilder CreateArray()
        {
            return new JsonArrayBuilder() { ConvertResultObject = ConvertResultObject };
        }

        public object CreateString(string value)
        {
            return ConvertResultObject(new JsonString(value));
        }

        public object CreateNumber(long value)
        {
            return ConvertResultObject(new JsonInteger(value));
        }

        public object CreateNumber(double value)
        {
            return ConvertResultObject(new JsonDouble(value));
        }

        public object CreateBool(bool value)
        {
            return ConvertResultObject(new JsonBool(value));
        }

        public object CreateDateTime(DateTime value)
        {
            return ConvertResultObject(new JsonDate(value));
        }

        public object CreateNull()
        {
            return ConvertResultObject(new JsonNull());
        }

        #endregion

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return _ => Instance;
        }

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateToSystemObjectTypeBuilderFactory()
        {
            return _ => ToSystemObjectInstance;
        }
    }
}