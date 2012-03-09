using System;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface IDbSession : IDatabaseCommandExecutor, IDatabaseCommandFactory, IDisposable
    {
        void Commit();
        void Rollback();
    }
}