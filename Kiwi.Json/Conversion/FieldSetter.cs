using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kiwi.Json.Conversion
{
    public class FieldSetter : IMemberSetter
    {
        private readonly FieldInfo _field;
        private readonly Action<object, object> _setter;

        public FieldSetter(FieldInfo field)
        {
            _field = field;
            _setter = CreateSetter(field);
        }

        #region IMemberSetter Members

        public string MemberName
        {
            get { return _field.Name; }
        }

        public Type MemberType
        {
            get { return _field.FieldType; }
        }

        public void SetValue(object instance, object memberValue)
        {
            _setter(instance, memberValue);
        }

        #endregion

        private static Action<object, object> CreateSetter(FieldInfo field)
        {
            var setter = new DynamicMethod(field.Name, typeof (void), new[] {typeof (object), typeof (object)},
                                           field.DeclaringType, true);
            var il = setter.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            if (field.FieldType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, field.FieldType);
            }
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);

            return (Action<object, object>) setter.CreateDelegate(typeof (Action<object, object>));
        }
    }
}