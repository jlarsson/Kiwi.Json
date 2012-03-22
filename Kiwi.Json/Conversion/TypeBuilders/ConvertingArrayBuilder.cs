using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ConvertingArrayBuilder : IArrayBuilder
    {
        private readonly IArrayBuilder _inner;
        private readonly Func<object, object> _convert;

        public ConvertingArrayBuilder(IArrayBuilder inner, Func<object, object> convert)
        {
            _inner = inner;
            _convert = convert;
        }

        public object CreateNewArray(ITypeBuilderRegistry registry, object instanceState)
        {
            return _inner.CreateNewArray(registry, instanceState);
        }

        public ITypeBuilder GetElementBuilder(ITypeBuilderRegistry registry)
        {
            return _inner.GetElementBuilder(registry);
        }

        public void AddElement(object array, object element)
        {
            _inner.AddElement(array, element);
        }

        public object GetArray(object array)
        {
            return _convert(_inner.GetArray(array));
        }
    }
}