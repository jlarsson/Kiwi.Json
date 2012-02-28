using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public abstract class AbstractTypeBuilder : ITypeBuilder
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

        private Exception CreateInvalidTypeException(string type)
        {
            throw new JsonSerializationException("Invalid occurence of " + type);
        }
    }
}