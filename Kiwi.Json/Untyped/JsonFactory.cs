using System;

namespace Kiwi.Json.Untyped
{
    public class JsonFactory : IJsonFactory
    {
        private readonly IJsonValue _false = new JsonBool(false);
        private readonly IJsonValue _null = new JsonNull();
        private readonly IJsonValue _true = new JsonBool(true);

        static JsonFactory()
        {
            Default = new JsonFactory();
        }

        public static IJsonFactory Default { get; private set; }

        #region IJsonFactory Members

        public IJsonObject CreateObject()
        {
            return new JsonObject();
        }

        public IJsonArray CreateArray()
        {
            return new JsonArray();
        }

        public IJsonValue CreateString(string value)
        {
            return new JsonString(value);
        }

        public IJsonValue CreateNumber(long value)
        {
            return new JsonInteger(value);
        }

        public IJsonValue CreateNumber(double value)
        {
            return new JsonDouble(value);
        }

        public IJsonValue CreateBool(bool value)
        {
            return value ? _true : _false;
        }

        public IJsonValue CreateDate(DateTime value)
        {
            return new JsonDate(value);
        }

        public IJsonValue CreateNull()
        {
            return _null;
        }

        #endregion
    }
}