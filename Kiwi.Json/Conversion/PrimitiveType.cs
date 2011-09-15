using System;

namespace Kiwi.Json.Conversion
{
    public class PrimitiveType<T> : IPrimitiveType<T>
    {
        public static readonly IPrimitiveType<T> Default = new PrimitiveType<T>();

        #region IPrimitiveType<T> Members

        public T Visit(object value, IPrimitiveValueVisitor<T> visitor)
        {
            if (value == null)
            {
                return visitor.VisitNull();
            }
            if (value is string)
            {
                return visitor.VisitString((string) value);
            }
            if ((value is int) || (value is long) || (value is uint) || (value is ulong) || (value is short) ||
                (value is ushort) || (value is byte) || (value is sbyte))
            {
                return visitor.VisitInteger(((IConvertible) value).ToInt64(null));
            }
            if (value is bool)
            {
                return visitor.VisitBool((bool)value);
            }
            if ((value is double) || (value is float) || (value is decimal))
            {
                return visitor.VisitDouble(((IConvertible) value).ToDouble(null));
            }
            if (value is DateTime)
            {
                return visitor.VisitDate((DateTime) value);
            }
            return visitor.VisitObject(value);
        }

        #endregion
    }
}