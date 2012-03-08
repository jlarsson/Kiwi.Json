using System;
using System.Data.Common;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public interface ITx: IDisposable
    {
        DbCommand CreateCommand();
        void Commit();
        void Rollback();
    }
}