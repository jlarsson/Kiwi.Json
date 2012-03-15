using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class NothingBuilder : ITypeBuilder, IArrayBuilder, IObjectBuilder
    {
        public static readonly ITypeBuilder Instance = new NothingBuilder();

        #region IArrayBuilder Members

        public object CreateNewArray(ITypeBuilderRegistry registry, object instanceState)
        {
            return null;
        }

        public ITypeBuilder GetElementBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        public void AddElement(object array, object element)
        {
        }

        public object GetArray(object array)
        {
            return null;
        }

        #endregion

        #region IObjectBuilder Members

        public object CreateNewObject(ITypeBuilderRegistry registry, object instanceState)
        {
            return null;
        }

        public object GetMemberState(string memberName, object unknown)
        {
            return null;
        }

        public ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            return this;
        }

        public void SetMember(string memberName, object o, object value)
        {
        }

        public object GetObject(object @object)
        {
            return null;
        }

        #endregion

        #region ITypeBuilder Members

        public IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        public IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        public object CreateString(ITypeBuilderRegistry registry, string value)
        {
            return null;
        }

        public object CreateNumber(ITypeBuilderRegistry registry, long value)
        {
            return null;
        }

        public object CreateNumber(ITypeBuilderRegistry registry, double value)
        {
            return null;
        }

        public object CreateBool(ITypeBuilderRegistry registry, bool value)
        {
            return null;
        }

        public object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue)
        {
            return null;
        }

        public object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }

        #endregion
    }
}