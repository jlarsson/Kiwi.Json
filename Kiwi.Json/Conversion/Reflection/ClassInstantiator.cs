using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kiwi.Json.Conversion.Reflection
{
    public class ClassInstantiator : IClassInstantiator
    {
        private readonly Func<object> _newInstance;

        public ClassInstantiator(Type classType, ConstructorInfo constructor)
        {
            var ctor = new DynamicMethod("construct", classType, Type.EmptyTypes);
            var il = ctor.GetILGenerator();
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);

            _newInstance = (Func<object>) ctor.CreateDelegate(typeof (Func<object>));
        }

        #region IClassInstantiator Members

        public object NewInstance()
        {
            return _newInstance();
        }

        #endregion
    }
}