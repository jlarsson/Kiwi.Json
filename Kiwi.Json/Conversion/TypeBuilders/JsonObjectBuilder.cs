using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonObjectBuilder : JsonValueBuilder, IObjectBuilder
    {
        #region IObjectBuilder Members

        public object CreateNewObject()
        {
            return new JsonObject();
        }

        public ITypeBuilder GetMemberBuilder(string memberName)
        {
            return this;
        }

        public void SetMember(string memberName, object @object, object value)
        {
            ((JsonObject)@object).Add(memberName, (IJsonValue) value);
        }

        public object GetObject(object @object)
        {
            return @object;
        }

        #endregion
    }
}