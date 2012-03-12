using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentSession : IDisposable
    {
        JET_SESID JetSesid { get; }
        Session Session { get; }
        JET_DBID JetDbid { get; }
        IEsentTransaction CreateTransaction();

        void CreateDatabase(string connect, CreateDatabaseGrbit grbit);
        void OpenDatabase(string connect, OpenDatabaseGrbit grbit);
        void AttachDatabase(AttachDatabaseGrbit grbit);
    }
}