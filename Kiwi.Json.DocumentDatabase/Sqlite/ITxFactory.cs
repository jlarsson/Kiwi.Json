namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public interface ITxFactory
    {
        ITx CreateTransaction();
    }
}