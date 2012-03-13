using Kiwi.Fluesent.Ddl;

namespace Kiwi.Fluesent
{
    public interface IEsentDatabase
    {
        string DatabasePath { get; }
        void SetCreateDatabaseOptions(IDatabaseDefinition databaseDefinition, bool alwaysCreate);
        IEsentSession CreateSession(bool attachAndOpenDatabase);
    }
}