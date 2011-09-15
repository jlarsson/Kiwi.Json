using System;

namespace Kiwi.Json.Conversion
{
    public interface IPrimitiveValueVisitor<T>
    {
        T VisitNull();
        T VisitBool(bool value);
        T VisitInteger(long value);
        T VisitString(string value);
        T VisitDate(DateTime value);
        T VisitDouble(double value);
        T VisitObject(object value);
    }
}