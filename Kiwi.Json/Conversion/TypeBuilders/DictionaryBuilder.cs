using System;
using System.Collections.Generic;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryBuilder<TDictionary, TValue> : AbstractTypeBuilder, IObjectBuilder
        where TDictionary : class, IDictionary<string, TValue>//, new()
    {
        private readonly ITypeBuilder _memberBuilder;
        private readonly IClassActivator _activator;

        public DictionaryBuilder(ITypeBuilder memberBuilder, IClassActivator activator)
        {
            _memberBuilder = memberBuilder;
            _activator = activator;
        }

        #region IObjectBuilder Members

        public override IObjectBuilder CreateObjectBuilder()
        {
            return this;
        }

        public override object CreateNewObject(object instanceState)
        {
            if (instanceState is TDictionary)
            {
                (instanceState as TDictionary).Clear();
                return instanceState;
            }
            return _activator.CreateInstance();
        }

        public override object GetMemberState(string memberName, object @object)
        {
            if (@object is TDictionary)
            {
                TValue state;
                if ((@object as TDictionary).TryGetValue(memberName, out state))
                {
                    return state;
                }
            }
            return null;
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
            var builder = new DictionaryBuilder<TDictionary, TValue>(registry.GetTypeBuilder<TValue>(), ClassActivator.Create(typeof(TDictionary)));
            return () => builder;
        }

        public override object CreateNull()
        {
            return null;
        }
    }
}