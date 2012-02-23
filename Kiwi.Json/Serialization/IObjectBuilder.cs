namespace Kiwi.Json.Serialization
{
    public interface IObjectBuilder
    {
        ITypeBuilder GetMemberBuilder(string memberName);
        void SetMember(string memberName, object value);
        object GetObject();
    }
}