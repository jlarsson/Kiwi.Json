namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface IAccessor
    {
        long Long(int index);
        string String(int index);
    }
}