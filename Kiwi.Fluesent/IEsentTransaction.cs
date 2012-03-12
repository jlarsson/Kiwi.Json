using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentTransaction : IDisposable
    {
        JET_SESID JetSesid { get; }
        IEsentSession Session { get; set; }
        void Commit(CommitTransactionGrbit grbit);
        void Rollback();
        void Pulse(CommitTransactionGrbit grbit);
        IEsentTable OpenTable(string name, OpenTableGrbit grbit);
    }
}