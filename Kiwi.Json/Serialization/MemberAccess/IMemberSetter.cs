using System;

namespace Kiwi.Json.Serialization.MemberAccess
{
    public interface IMemberSetter
    {
        string MemberName { get; }
        Type MemberType { get; }
        void SetValue(object instance, object memberValue);
    }
}