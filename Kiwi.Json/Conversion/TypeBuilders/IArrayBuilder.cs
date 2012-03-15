namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface IArrayBuilder
    {
        object CreateNewArray(ITypeBuilderRegistry registry, object instanceState);
        ITypeBuilder GetElementBuilder(ITypeBuilderRegistry registry);
        void AddElement(object array, object element);
        object GetArray(object array);
    }
}