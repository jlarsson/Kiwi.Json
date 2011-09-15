using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kiwi.Json.Conversion.Reflection
{
    public class PropertySetter : IMemberSetter
    {
        private readonly PropertyInfo _property;
        private readonly Action<object, object> _setter;

        public PropertySetter(PropertyInfo property)
        {
            _property = property;
            _setter = CreateSetter(property);
        }

        #region IMemberSetter Members

        public string MemberName
        {
            get { return _property.Name; }
        }

        public Type MemberType
        {
            get { return _property.PropertyType; }
        }

        public void SetValue(object instance, object memberValue)
        {
            _setter(instance, memberValue);
        }

        #endregion

        private static Action<object, object> CreateSetter(PropertyInfo property)
        {
            var setter = new DynamicMethod(property.Name, typeof (void), new[] {typeof (object), typeof (object)},
                                           property.DeclaringType, true);
            var il = setter.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, property.DeclaringType);
            il.Emit(OpCodes.Ldarg_1);

            if (property.PropertyType.IsClass)
            {
                il.Emit(OpCodes.Castclass, property.PropertyType);
            }
            else
            {
                il.Emit(OpCodes.Unbox_Any, property.PropertyType);
            }

            il.EmitCall(OpCodes.Callvirt, property.GetSetMethod(), null);
            il.Emit(OpCodes.Ret);

            return (Action<object, object>) setter.CreateDelegate(typeof (Action<object, object>));
        }
    }
}