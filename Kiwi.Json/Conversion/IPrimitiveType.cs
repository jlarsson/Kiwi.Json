namespace Kiwi.Json.Conversion
{
    public interface IPrimitiveType<T>
    {
        T Visit(object value, IPrimitiveValueVisitor<T> visitor);
    }
}