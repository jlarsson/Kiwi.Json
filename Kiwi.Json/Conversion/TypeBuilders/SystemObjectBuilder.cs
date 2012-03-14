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
            _dictionaryBuilder = new DictionaryBuilder<Dictionary<string, object>, object>(this, new ClassActivator<Dictionary<string, object>>());
            _arrayBuilder = new CollectionBuilder<List<object>, object>(this);
        }

        #region ITypeBuilder Members

        public IObjectBuilder CreateObjectBuilder()
        {
            return _dictionaryBuilder;
        }

        public IArrayBuilder CreateArrayBuilder()
        {
            return _arrayBuilder;
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

        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var builder = new SystemObjectBuilder();
            return () => builder;
        }
    }
}