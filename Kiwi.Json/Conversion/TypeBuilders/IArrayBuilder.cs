namespace Kiwi.Json.Conversion.TypeBuilders
{
    public interface IArrayBuilder
    {
        ITypeBuilder GetElementBuilder();
        void AddElement(object element);
        object GetArray();
    }
}