using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class JsonArrayBuilder : JsonValueBuilder, IArrayBuilder
    {
        private readonly JsonArray _array = new JsonArray();

        #region IArrayBuilder<IJsonValue> Members

        public ITypeBuilder GetElementBuilder()
        {
            return this;
        }

        public void AddElement(object element)
        {
            _array.Add((IJsonValue)element);
        }

        public object GetObject()
        {
            return _array;
        }

        #endregion
    }
}