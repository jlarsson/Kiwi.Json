namespace Kiwi.Json.Untyped
{
    public interface IJsonValueVisitor<out T>
    {
        T VisitArray(IJsonArray value);
        T VisitBool(IJsonBool value);
        T VisitDate(IJsonDate value);
        T VisitDouble(IJsonDouble value);
        T VisitInteger(IJsonInteger value);
        T VisitNull(IJsonNull value);
        T VisitObject(IJsonObject value);
        T VisitString(IJsonString value);
    }
}