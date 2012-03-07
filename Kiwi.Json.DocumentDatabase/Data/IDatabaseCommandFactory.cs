namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface IDatabaseCommandFactory
    {
        IDatabaseCommand CreateSqlCommand(string sql);
    }
}
