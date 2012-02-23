namespace Kiwi.Json.Serialization.MemberAccess
{
    public interface IMemberGetter
    {
        string MemberName { get; }
        object GetMemberValue(object instance);
    }
}