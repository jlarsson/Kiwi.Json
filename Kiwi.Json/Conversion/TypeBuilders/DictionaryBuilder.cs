using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryBuilder<TDictionary, TValue>: AbstractTypeBuilder, IObjectBuilder where TDictionary: class, IDictionary<string,TValue>, new()
    {
        private readonly ITypeBuilderRegistry _registry;
        private readonly TDictionary _instance;
        private ITypeBuilder _memberBuilder;

        public DictionaryBuilder(ITypeBuilderRegistry registry)
        {
            _registry = registry;
            _instance = new TDictionary();
        }

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new DictionaryBuilder<TDictionary, TValue>(r);
        }

        public override IObjectBuilder CreateObject()
        {
            return new DictionaryBuilder<TDictionary, TValue>(_registry);
        }

        public override object CreateNull()
        {
            return null;
        }

        public ITypeBuilder GetMemberBuilder(string memberName)
        {
            return _memberBuilder ?? (_memberBuilder = _registry.GetTypeBuilder<TValue>());
        }

        public void SetMember(string memberName, object value)
        {
            _instance.Add(memberName, (TValue)value);
        }

        public object GetObject()
        {
            return _instance;
        }
    }
}