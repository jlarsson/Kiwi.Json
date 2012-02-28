using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonArrayBuilder : JsonValueBuilder, IArrayBuilder
    {
        private readonly JsonArray _array = new JsonArray();

        #region IArrayBuilder Members

        public ITypeBuilder GetElementBuilder()
        {
            return this;
        }

        public void AddElement(object element)
        {
            _array.Add((IJsonValue) element);
        }

        public object GetArray()
        {
            return _array;
        }

        #endregion

        public new static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new TypeBuilderFactory()
            {
                OnCreateNull = () => null,
                OnCreateArray= () => new JsonArrayBuilder()
            };
        }
    }
}