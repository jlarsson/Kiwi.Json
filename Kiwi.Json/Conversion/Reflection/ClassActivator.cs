using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kiwi.Json.Conversion.Reflection
{
    public class ClassActivator<TClass> : IClassActivator where TClass : new()
    {
        public object CreateInstance()
        {
            return new TClass();
        }
    }

    public class ClassActivator : IClassActivator
    {
        private Func<object, object> _activator;

        public ClassActivator(ConstructorInfo constructor)
        {
            _activator = CreateActivator(constructor);
        }

        private Func<object, object> CreateActivator(ConstructorInfo constructorInfo)
        {
            var getter = new DynamicMethod("", typeof(object), new[] { typeof(object) },
                                           constructorInfo.DeclaringType, true);
            var il = getter.GetILGenerator();
            il.Emit(OpCodes.Newobj, constructorInfo);
            il.Emit(OpCodes.Ret);
            return (Func<object, object>)getter.CreateDelegate(typeof(Func<object, object>));
        }

        public object CreateInstance()
        {
            return _activator(null);
        }

        public static IClassActivator Create(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            return constructor == null
                                            ? new MissingDefaultClassActivator(type) as IClassActivator
                                            : new ClassActivator(constructor);
        }
    }
}