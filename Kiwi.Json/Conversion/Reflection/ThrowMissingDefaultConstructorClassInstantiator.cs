using System;

namespace Kiwi.Json.Conversion.Reflection
{
    public class ThrowMissingDefaultConstructorClassInstantiator : IClassInstantiator
    {
        private readonly Type _classType;

        public ThrowMissingDefaultConstructorClassInstantiator(Type classType)
        {
            _classType = classType;
        }

        #region IClassInstantiator Members

        public object NewInstance()
        {
            throw new MissingDefaultConstructorException(_classType);
        }

        #endregion
    }
}