using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class SystemObjectBuilder : ITypeBuilder
    {
        private readonly ITypeBuilderRegistry _registry;

        public SystemObjectBuilder(ITypeBuilderRegistry registry)
        {
            _registry = registry;
        }

        #region ITypeBuilder Members

        public IObjectBuilder CreateObject()
        {
            return new DictionaryBuilder<Dictionary<string, object>, object>(_registry);
        }

        public IArrayBuilder CreateArray()
        {
            return new ListBuilder<List<object>, object>(_registry);
        }

        public object CreateString(string value)
        {
            return value;
        }

        public object CreateNumber(long value)
        {
            return (int)value;
        }

        public object CreateNumber(double value)
        {
            return value;
        }

        public object CreateBool(bool value)
        {
            return value;
        }

        public object CreateDateTime(DateTime value)
        {
            return value;
        }

        public object CreateNull()
        {
            return null;
        }

        #endregion

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new SystemObjectBuilder(r);
        }
    }
}