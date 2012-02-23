using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kiwi.Json.Serialization.MemberAccess
{
    public class PropertyGetter : IMemberGetter
    {
        private readonly Func<object, object> _getter;
        private readonly PropertyInfo _property;

        public PropertyGetter(PropertyInfo property)
        {
            _property = property;
            _getter = CreateGetter(_property);
        }

        #region IMemberGetter Members

        public string MemberName
        {
            get { return _property.Name; }
        }

        public object GetMemberValue(object instance)
        {
            return _getter(instance);
        }

        #endregion

        private static Func<object, object> CreateGetter(PropertyInfo property)
        {
            var getter = new DynamicMethod(property.Name, typeof (object), new[] {typeof (object)},
                                           property.DeclaringType, true);
            var il = getter.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, property.DeclaringType);
            il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
            if (!property.PropertyType.IsClass)
            {
                il.Emit(OpCodes.Box, property.PropertyType);
            }
            il.Emit(OpCodes.Ret);
            return (Func<object, object>) getter.CreateDelegate(typeof (Func<object, object>));
        }
    }
}