using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public abstract class AbstractTypeBuilder : ITypeBuilder, IArrayBuilder, IObjectBuilder
    {
        #region ITypeBuilder Members

        public virtual IObjectBuilder CreateObject()
        {
            throw CreateInvalidTypeException("object");
        }

        public virtual IArrayBuilder CreateArray()
        {
            throw CreateInvalidTypeException("array");
        }

        public virtual object CreateString(string value)
        {
            throw CreateInvalidTypeException("string");
        }

        public virtual object CreateNumber(long value)
        {
            throw CreateInvalidTypeException("integer");
        }

        public virtual object CreateNumber(double value)
        {
            throw CreateInvalidTypeException("floating point number");
        }

        public virtual object CreateBool(bool value)
        {
            throw CreateInvalidTypeException("boolean");
        }

        public virtual object CreateDateTime(DateTime value)
        {
            throw CreateInvalidTypeException("date");
        }

        public virtual object CreateNull()
        {
            throw CreateInvalidTypeException("null");
        }

        #endregion

        #region IArrayBuilder Members

        public virtual ITypeBuilder GetElementBuilder()
        {
            throw CreateInvalidCallException("array element");
        }

        public virtual void AddElement(object element)
        {
            throw CreateInvalidCallException("array element");
        }

        public virtual object GetArray()
        {
            throw CreateInvalidCallException("array");
        }

        #endregion

        #region IObjectBuilder Members

        public virtual ITypeBuilder GetMemberBuilder(string memberName)
        {
            throw CreateInvalidCallException("object member");
        }

        public virtual void SetMember(string memberName, object value)
        {
            throw CreateInvalidCallException("object member");
        }

        public virtual object GetObject()
        {
            throw CreateInvalidCallException("object");
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