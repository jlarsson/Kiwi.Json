using System;

namespace Kiwi.Json.Conversion
{
    public interface IMemberSetter
    {
        string MemberName { get; }
        Type MemberType { get; }
        void SetValue(object instance, object memberValue);
    }
}