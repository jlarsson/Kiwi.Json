namespace Kiwi.Json.Serialization
{
    public interface IArrayBuilder
    {
        ITypeBuilder GetElementBuilder();
        void AddElement(object element);
        object GetObject();
    }
}