using System;
using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Converters
{
    public abstract class AbstractJsonConverter : IJsonConverter
    {
        #region IJsonConverter Members

        public abstract ITypeBuilder CreateTypeBuilder(Type type);
        public abstract ITypeWriter CreateTypeWriter(Type type);

        #endregion

        protected ITypeBuilder TryCreateTypeBuilder<TReal, TProxy>(Type type, Func<TProxy, TReal> convert)
        {
            if (typeof (TReal).IsAssignableFrom(type))
            {
                return new ProxyBuilder<object>(jsonValue =>
                                                    {
                                                        var proxy = jsonValue.ToObject<TProxy>();
                                                        return convert(proxy);
                                                    });
            }
            return null;
        }

        protected ITypeWriter TryCreateWriter<T>(Type type, Func<T, object> createProxy)
        {
            if (typeof (T).IsAssignableFrom(type))
            {
                return new ProxyWriter<T>(createProxy);
            }
            return null;
        }

        #region Nested type: ProxyBuilder

        public class ProxyBuilder<T> : ConvertingTypeBuilder
        {
            public ProxyBuilder(Func<IJsonValue, T> convert)
                : base(new JsonValueBuilder(), o => convert((IJsonValue) o))
            {
            }
        }

        #endregion

        #region Nested type: ProxyWriter

        public class ProxyWriter<T> : ITypeWriter
        {
            private readonly Func<T, object> _createProxy;

            public ProxyWriter(Func<T, object> createProxy)
            {
                _createProxy = createProxy;
            }

            #region ITypeWriter Members

            public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
            {
                registry.Write(writer, _createProxy((T) value));
            }

            #endregion
        }

        #endregion
    }
}