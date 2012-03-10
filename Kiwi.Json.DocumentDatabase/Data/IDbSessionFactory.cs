namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface IDbSessionFactory
    {
        IDbSession CreateSession();
    }
}