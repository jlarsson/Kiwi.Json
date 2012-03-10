using System;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface IDbSession : IDatabaseCommandFactory, IDisposable
    {
        void Commit();
        void Rollback();
    }
}