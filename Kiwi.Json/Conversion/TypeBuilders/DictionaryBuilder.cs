using System.Collections.Generic;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryBuilder<TDictionary, TValue> : AbstractTypeBuilder, IObjectBuilder
        where TDictionary : class, IDictionary<string, TValue> //, new()
    {
// ReSharper disable StaticFieldInGenericType
        private static readonly IClassActivator Activator = ClassActivator.Create(typeof (TDictionary));
// ReSharper restore StaticFieldInGenericType

        #region IObjectBuilder Members

        public override object CreateNewObject(ITypeBuilderRegistry registry, object instanceState)
        {
            if (instanceState is TDictionary)
            {
                (instanceState as TDictionary).Clear();
                return instanceState;
            }
            return Activator.CreateInstance();
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

        public override ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            return registry.GetTypeBuilder<TValue>();
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

        public override IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        public override object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }
    }
}