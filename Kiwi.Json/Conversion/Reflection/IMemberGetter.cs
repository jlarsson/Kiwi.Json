namespace Kiwi.Json.Conversion.Reflection
{
    public interface IMemberGetter
    {
        string MemberName { get; }
        object GetMemberValue(object instance);
    }
}