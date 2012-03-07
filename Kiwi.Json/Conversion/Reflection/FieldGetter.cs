using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kiwi.Json.Conversion.Reflection
{
    public class FieldGetter : IMemberGetter
    {
        private readonly FieldInfo _field;
        private readonly Func<object, object> _getter;

        public FieldGetter(FieldInfo field)
        {
            _field = field;
            _getter = CreateGetter(_field);
        }

        #region IMemberGetter Members

        public string MemberName
        {
            get { return _field.Name; }
        }

        public object GetMemberValue(object instance)
        {
            return _getter(instance);
        }

        #endregion

        private static Func<object, object> CreateGetter(FieldInfo field)
        {
            var getter = new DynamicMethod(field.Name, typeof (object), new[] {typeof (object)}, field.DeclaringType,
                                           true);
            var il = getter.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);

            if (field.DeclaringType.IsValueType)
            {
                il.Emit(OpCodes.Unbox, field.DeclaringType);
            }
            il.Emit(OpCodes.Ldfld, field);
            if (field.FieldType.IsValueType)
            {
                il.Emit(OpCodes.Box, field.FieldType);
            }
            il.Emit(OpCodes.Ret);

            return (Func<object, object>) getter.CreateDelegate(typeof (Func<object, object>));
        }
    }
}