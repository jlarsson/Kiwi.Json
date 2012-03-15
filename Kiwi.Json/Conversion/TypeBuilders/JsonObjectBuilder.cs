using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonObjectBuilder : JsonValueBuilder, IObjectBuilder
    {
        #region IObjectBuilder Members

        public object CreateNewObject(ITypeBuilderRegistry registry, object instanceState)
        {
            if (instanceState is IJsonObject)
            {
                return instanceState as IJsonObject;
            }
            return new JsonObject();
        }

        public object GetMemberState(string memberName, object instanceState)
        {
            if (instanceState is IJsonObject)
            {
                IJsonValue value;
                return (instanceState as IJsonObject).TryGetValue(memberName, out value) ? value : null;
            }

            return null;
        }

        public ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            return this;
        }

        public void SetMember(string memberName, object @object, object value)
        {
            ((JsonObject)@object)[memberName] = (IJsonValue) value;
        }

        public object GetObject(object @object)
        {
            return @object;
        }

        #endregion
    }
}