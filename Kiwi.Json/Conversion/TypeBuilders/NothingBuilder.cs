using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class NothingBuilder : ITypeBuilder, IArrayBuilder, IObjectBuilder
    {
        public static readonly ITypeBuilder Instance = new NothingBuilder();

        #region IArrayBuilder Members

        public ITypeBuilder GetElementBuilder()
        {
            return this;
        }

        public void AddElement(object element)
        {
        }

        public object GetArray()
        {
            return null;
        }

        #endregion

        #region IObjectBuilder Members

        public ITypeBuilder GetMemberBuilder(string memberName)
        {
            return this;
        }

        public void SetMember(string memberName, object value)
        {
        }

        public object GetObject()
        {
            return null;
        }

        #endregion

        #region ITypeBuilder Members

        public IObjectBuilder CreateObject()
        {
            return this;
        }

        public IArrayBuilder CreateArray()
        {
            return this;
        }

        public object CreateString(string value)
        {
            return null;
        }

        public object CreateNumber(long value)
        {
            return null;
        }

        public object CreateNumber(double value)
        {
            return null;
        }

        public object CreateBool(bool value)
        {
            return null;
        }

        public object CreateDateTime(DateTime value)
        {
            return null;
        }

        public object CreateNull()
        {
            return null;
        }

        #endregion
    }
}