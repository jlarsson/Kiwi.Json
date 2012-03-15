using System;

namespace Kiwi.Json.Conversion.Reflection
{
    public class ClassActivator<TClass> : IClassActivator where TClass : new()
    {
        #region IClassActivator Members

        public object CreateInstance()
        {
            return new TClass();
        }

        #endregion
    }

    public static class ClassActivator
    {
        public static IClassActivator Create(Type type)
        {
            if (!type.IsClass)
            {
                return new NullClassActivator();
            }
            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
            {
                return new MissingDefaultClassActivator(type);
            }

            //return (IClassActivator)typeof (ClassActivator<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            return new ConstructorInfoClassActivator(constructor);
        }
    }

    public class NullClassActivator : IClassActivator
    {
        #region IClassActivator Members

        public object CreateInstance()
        {
            return null;
        }

        #endregion
    }
}