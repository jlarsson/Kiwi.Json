using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public abstract class AbstractTypeBuilder : ITypeBuilder, IArrayBuilder, IObjectBuilder
    {
        #region IArrayBuilder Members

        public virtual object CreateNewArray(ITypeBuilderRegistry registry, object instanceState)
        {
            throw CreateInvalidCallException("new array");
        }

        public virtual ITypeBuilder GetElementBuilder(ITypeBuilderRegistry registry)
        {
            throw CreateInvalidCallException("array element");
        }

        public virtual void AddElement(object array, object element)
        {
            throw CreateInvalidCallException("array element");
        }

        public virtual object GetArray(object array)
        {
            throw CreateInvalidCallException("array");
        }

        #endregion

        #region IObjectBuilder Members

        public virtual object CreateNewObject(ITypeBuilderRegistry registry, object instanceState)
        {
            throw CreateInvalidCallException("new object");
        }

        public virtual object GetMemberState(string memberName, object @object)
        {
            throw CreateInvalidCallException("member state");
        }

        public virtual ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            throw CreateInvalidCallException("object member");
        }

        public virtual void SetMember(string memberName, object o, object value)
        {
            throw CreateInvalidCallException("object member");
        }

        public virtual object GetObject(object @object)
        {
            throw CreateInvalidCallException("object");
        }

        #endregion

        #region ITypeBuilder Members

        public virtual IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            throw CreateInvalidTypeException("object");
        }

        public virtual IArrayBuilder CreateArrayBuilder(ITypeBuilderRegistry registry)
        {
            throw CreateInvalidTypeException("array");
        }

        public virtual object CreateString(ITypeBuilderRegistry registry, string value)
        {
            throw CreateInvalidTypeException("string");
        }

        public virtual object CreateNumber(ITypeBuilderRegistry registry, long value)
        {
            throw CreateInvalidTypeException("integer");
        }

        public virtual object CreateNumber(ITypeBuilderRegistry registry, double value)
        {
            throw CreateInvalidTypeException("floating point number");
        }

        public virtual object CreateBool(ITypeBuilderRegistry registry, bool value)
        {
            throw CreateInvalidTypeException("boolean");
        }

        public virtual object CreateDateTime(ITypeBuilderRegistry registry, DateTime value, object sourceValue)
        {
            throw CreateInvalidTypeException("date");
        }

        public virtual object CreateNull(ITypeBuilderRegistry registry)
        {
            throw CreateInvalidTypeException("null");
        }

        #endregion

        private Exception CreateInvalidCallException(string type)
        {
            throw new JsonSerializationException("Invalid context: " + type);
        }

        private Exception CreateInvalidTypeException(string type)
        {
            throw new JsonSerializationException("Invalid occurence of " + type);
        }
    }
}