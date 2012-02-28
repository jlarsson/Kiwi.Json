using System;

namespace Kiwi.Json.Conversion.Reflection
{
    public interface IMemberSetter
    {
        string MemberName { get; }
        Type MemberType { get; }
        void SetValue(object instance, object memberValue);
    }
}