using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentSession : IDisposable
    {
        JET_DBID JetDbid { get; }
        JET_SESID JetSesid { get; }

        void LockWrites();
        IEsentTransaction CreateTransaction();
        void CreateDatabase(string connect, CreateDatabaseGrbit grbit);
        void OpenDatabase(string connect, OpenDatabaseGrbit grbit);
        void AttachDatabase(AttachDatabaseGrbit grbit);
    }
}