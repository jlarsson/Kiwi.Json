using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryBuilder<TDictionary, TValue> : AbstractTypeBuilder, IObjectBuilder
        where TDictionary : class, IDictionary<string, TValue>, new()
    {
        private readonly ITypeBuilder _memberBuilder;

        public DictionaryBuilder(ITypeBuilder memberBuilder)
        {
            _memberBuilder = memberBuilder;
        }

        #region IObjectBuilder Members

        public override IObjectBuilder CreateObjectBuilder()
        {
            return this;
        }

        public override object CreateNewObject()
        {
            return new TDictionary();
        }

        public override ITypeBuilder GetMemberBuilder(string memberName)
        {
            return _memberBuilder;
        }

        public override void SetMember(string memberName, object @object, object value)
        {
            ((TDictionary) @object).Add(memberName, (TValue) value);
        }

        public override object GetObject(object @object)
        {
            return @object;
        }

        #endregion

        public static Func<ITypeBuilder> CreateTypeBuilderFactory(ITypeBuilderRegistry registry)
        {
            var builder = new DictionaryBuilder<TDictionary, TValue>(registry.GetTypeBuilder<TValue>());
            return () => builder;
        }

        public override object CreateNull()
        {
            return null;
        }
    }
}