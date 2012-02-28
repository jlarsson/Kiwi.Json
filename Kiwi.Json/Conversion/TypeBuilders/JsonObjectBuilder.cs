using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonObjectBuilder : JsonValueBuilder, IObjectBuilder
    {
        private readonly JsonObject _object = new JsonObject();

        #region IObjectBuilder Members

        public ITypeBuilder GetMemberBuilder(string memberName)
        {
            return this;
        }

        public void SetMember(string memberName, object value)
        {
            _object.Add(memberName, (IJsonValue) value);
        }

        public object GetObject()
        {
            return _object;
        }

        #endregion

        public new static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new TypeBuilderFactory()
            {
                OnCreateNull = () => null,
                OnCreateObject = () => new JsonObjectBuilder()
            };
        }
    }
}