using System;
using System.Collections.Generic;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryBuilder<TDictionary, TValue>: AbstractTypeBuilder, IObjectBuilder where TDictionary: class, IDictionary<string,TValue>, new()
    {
        private readonly ITypeBuilderRegistry _registry;
        private readonly TDictionary _instance = new TDictionary();
        private ITypeBuilder _memberBuilder;

        public DictionaryBuilder(ITypeBuilderRegistry registry)
        {
            _registry = registry;
        }

        public static Func<ITypeBuilderRegistry, ITypeBuilder> CreateTypeBuilderFactory()
        {
            return r => new TypeBuilderFactory()
                            {
                                OnCreateNull = () => null,
                                OnCreateObject = () => new DictionaryBuilder<TDictionary, TValue>(r)
                            };
        }

        public override ITypeBuilder GetMemberBuilder(string memberName)
        {
            return _memberBuilder ?? (_memberBuilder = _registry.GetTypeBuilder<TValue>());
        }

        public override void SetMember(string memberName, object value)
        {
            _instance.Add(memberName, (TValue)value);
        }

        public override object GetObject()
        {
            return _instance;
        }
    }
}