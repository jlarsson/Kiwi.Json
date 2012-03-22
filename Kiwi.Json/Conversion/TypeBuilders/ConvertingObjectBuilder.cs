using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ConvertingObjectBuilder : IObjectBuilder
    {
        private readonly Func<object, object> _convert;
        private readonly IObjectBuilder _inner;

        public ConvertingObjectBuilder(IObjectBuilder inner, Func<object, object> convert)
        {
            _inner = inner;
            _convert = convert;
        }

        #region IObjectBuilder Members

        public object CreateNewObject(ITypeBuilderRegistry registry, object instanceState)
        {
            return _inner.CreateNewObject(registry, instanceState);
        }

        public object GetMemberState(string memberName, object @object)
        {
            return _inner.GetMemberState(memberName, @object);
        }

        public ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            return _inner.GetMemberBuilder(registry, memberName);
        }

        public void SetMember(string memberName, object @object, object value)
        {
            _inner.SetMember(memberName, @object, value);
        }

        public object GetObject(object @object)
        {
            return _convert(_inner.GetObject(@object));
        }

        #endregion
    }
}