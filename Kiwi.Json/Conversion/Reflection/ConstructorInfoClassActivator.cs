using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kiwi.Json.Conversion.Reflection
{
    public class ConstructorInfoClassActivator : IClassActivator
    {
        private readonly Func<object, object> _activator;

        public ConstructorInfoClassActivator(ConstructorInfo constructor)
        {
            _activator = CreateActivator(constructor);
        }

        #region IClassActivator Members

        public object CreateInstance()
        {
            return _activator(null);
        }

        #endregion

        private Func<object, object> CreateActivator(ConstructorInfo constructorInfo)
        {
            var getter = new DynamicMethod("", typeof (object), new[] {typeof (object)},
                                           constructorInfo.DeclaringType, true);
            var il = getter.GetILGenerator();
            il.Emit(OpCodes.Newobj, constructorInfo);
            il.Emit(OpCodes.Ret);
            return (Func<object, object>) getter.CreateDelegate(typeof (Func<object, object>));
        }
    }
}