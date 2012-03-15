using System;

namespace Kiwi.Json.Conversion.Reflection
{
    public class MissingDefaultClassActivator : IClassActivator
    {
        private readonly Type _type;

        public MissingDefaultClassActivator(Type type)
        {
            _type = type;
        }

        #region IClassActivator Members

        public object CreateInstance()
        {
            throw new InvalidClassForDeserializationException(
                string.Format("Cannot create instance of {0} since it has no public default constructor defined",
                              _type));
        }

        #endregion
    }
}