using System;
using System.Collections.Generic;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class SystemObjectBuilder : ITypeBuilder
    {
        private readonly IObjectBuilder _dictionaryBuilder;
        private readonly IArrayBuilder _arrayBuilder;

        public SystemObjectBuilder()
        {
            _dictionaryBuilder = new DictionaryBuilder<Dictionary<string, object>, object>(new ClassActivator<Dictionary<string, object>>());
            _arrayBuilder = new CollectionBuilder<List<object>, object>();
        }

        #region ITypeBuilder Members

        public IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            return _dictionaryBuilder;
        }

        public IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
        {
            return _arrayBuilder;
        }

        public object CreateString(ITypeBuilderRegistry registry, string value)
        {
            return value;
        }

        public object CreateNumber(ITypeBuilderRegistry registry, long value)
        {
            return (int)value;
        }

        public object CreateNumber(ITypeBuilderRegistry registry, double value)
        {
            return value;
        }

        public object CreateBool(ITypeBuilderRegistry registry, bool value)
        {
            return value;
        }

        public object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue)
        {
            return value;
        }

        public object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }

        #endregion

        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var builder = new SystemObjectBuilder();
            return () => builder;
        }
    }
}