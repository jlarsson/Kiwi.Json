using System;
using System.Collections.Generic;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;

namespace Kiwi.Json.Converters
{
    public class InterningStringConverter: AbstractJsonConverter
    {
        public override ITypeBuilder CreateTypeBuilder(Type type)
        {
            return type == typeof(string) ? new StringTypeBuilder() : null;
        }

        public override ITypeWriter CreateTypeWriter(Type type)
        {
            return null;
        }

        public class StringTypeBuilder : ITypeBuilder
        {
            readonly Dictionary<string, string> _interned = new Dictionary<string, string>();

            #region ITypeBuilder Members

            public IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
            {
                throw new NotImplementedException();
            }

            public object CreateBool(ITypeBuilderRegistry registry, bool value)
            {
                throw new NotImplementedException();
            }

            public object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue)
            {
                throw new NotImplementedException();
            }

            public object CreateNull(ITypeBuilderRegistry registry)
            {
                throw new NotImplementedException();
            }

            public object CreateNumber(ITypeBuilderRegistry registry, double value)
            {
                throw new NotImplementedException();
            }

            public object CreateNumber(ITypeBuilderRegistry registry, long value)
            {
                throw new NotImplementedException();
            }

            public IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
            {
                throw new NotImplementedException();
            }

            public object CreateString(ITypeBuilderRegistry registry, string value)
            {
                string interned;
                if (!_interned.TryGetValue(value, out interned))
                {
                    interned = value;
                    _interned.Add(value, interned);
                }
                return interned;
            }

            #endregion
        }
    }
}