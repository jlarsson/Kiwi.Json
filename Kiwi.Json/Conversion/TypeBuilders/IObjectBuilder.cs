namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface IObjectBuilder
    {
        object CreateNewObject(ITypeBuilderRegistry registry, object instanceState);
        object GetMemberState(string memberName, object @object); 
        ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName);
        void SetMember(string memberName, object @object, object value);
        object GetObject(object @object);
    }
}