namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface IArrayBuilder
    {
        object CreateNewArray(object instanceState);
        ITypeBuilder GetElementBuilder();
        void AddElement(object array, object element);
        object GetArray(object array);
    }
}