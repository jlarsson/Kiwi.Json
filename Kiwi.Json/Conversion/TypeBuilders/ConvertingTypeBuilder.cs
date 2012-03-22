using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public abstract class ConvertingTypeBuilder : ITypeBuilder
    {
        private readonly Func<object, object> _convert;
        private readonly ITypeBuilder _inner;

        protected ConvertingTypeBuilder(ITypeBuilder inner, Func<object, object> convert)
        {
            _inner = inner;
            _convert = convert;
        }

        #region ITypeBuilder Members

        public IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            var builder = _inner.CreateObjectBuilder(registry);
            return builder == null ? null : new ConvertingObjectBuilder(builder, _convert);
        }

        public IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
        {
            var builder = _inner.CreateArrayBuilder(registry);
            return builder == null ? null : new ConvertingArrayBuilder(builder, _convert);
        }

        public object CreateString(ITypeBuilderRegistry registry, string value)
        {
            return _convert(_inner.CreateString(registry, value));
        }

        public object CreateNumber(ITypeBuilderRegistry registry, long value)
        {
            return _convert(_inner.CreateNumber(registry, value));
        }

        public object CreateNumber(ITypeBuilderRegistry registry, double value)
        {
            return _convert(_inner.CreateNumber(registry, value));
        }

        public object CreateBool(ITypeBuilderRegistry registry, bool value)
        {
            return _convert(_inner.CreateBool(registry, value));
        }

        public object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue)
        {
            return _convert(_inner.CreateDateTime(registry, value, sourceValue));
        }

        public object CreateNull(ITypeBuilderRegistry registry)
        {
            return _convert(_inner.CreateNull(registry));
        }

        #endregion
    }
}