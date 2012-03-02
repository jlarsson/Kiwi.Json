using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class NothingBuilder : ITypeBuilder, IArrayBuilder, IObjectBuilder
    {
        public static readonly ITypeBuilder Instance = new NothingBuilder();

        #region IArrayBuilder Members

        public object CreateNewArray()
        {
            return null;
        }

        public ITypeBuilder GetElementBuilder()
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

        public object CreateNewObject()
        {
            return null;
        }

        public ITypeBuilder GetMemberBuilder(string memberName)
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

        public IObjectBuilder CreateObjectBuilder()
        {
            return this;
        }

        public IArrayBuilder CreateArrayBuilder()
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