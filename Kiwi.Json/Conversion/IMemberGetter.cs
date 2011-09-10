namespace Kiwi.Json.Conversion
{
    public interface IMemberGetter
    {
        string MemberName { get; }
        object GetMemberValue(object instance);
    }
}