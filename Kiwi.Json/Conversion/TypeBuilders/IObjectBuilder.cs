namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface IObjectBuilder
    {
        object CreateNewObject();
        ITypeBuilder GetMemberBuilder(string memberName);
        void SetMember(string memberName, object @object, object value);
        object GetObject(object @object);
    }
}