using System;
using System.Collections.Generic;
using Kiwi.Json.Conversion.Reflection;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryBuilder<TDictionary, TKey, TValue> : AbstractTypeBuilder
        where TDictionary : class, IDictionary<TKey, TValue>
    {
// ReSharper disable StaticFieldInGenericType
        private static readonly IClassActivator Activator;

        private static Func<string, TKey> ConvertMemberToKey;
// ReSharper restore StaticFieldInGenericType

        static DictionaryBuilder()
        {
            Activator = ClassActivator.Create(typeof(TDictionary));

            if (typeof(TKey) == typeof(string))
            {
                ConvertMemberToKey = ConvertMemberNameToStringKey;
            }
            else
            {
                ConvertMemberToKey = ConvertMemberNameToKey;
            }
        }

        private static TKey ConvertMemberNameToStringKey(string memberName)
        {
            return (TKey)(object)memberName;
        }
        private static TKey ConvertMemberNameToKey(string memberName)
        {
            try
            {
                return (TKey) Convert.ChangeType(memberName, typeof (TKey));
            }
            catch(Exception)
            {
                throw new InvalidClassForDeserializationException(
                    string.Format(
                        "Cannot deserialize json into dictionary of type {0}. A string with value {1} is not convertible to a key of type {2}.",
                        typeof(TDictionary),
                        memberName,
                        typeof(TKey)));
                
            }
        }
        
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
                if ((@object as TDictionary).TryGetValue(ConvertMemberToKey(memberName), out state))
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
            ((TDictionary) @object).Add(ConvertMemberToKey(memberName), (TValue) value);
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

        protected override Type BuildType
        {
            get { return typeof(TDictionary); }
        }
    }
}