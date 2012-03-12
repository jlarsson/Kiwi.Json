using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentTransaction : IEsentTransaction
    {
        protected Transaction Transaction;

        public EsentTransaction(IEsentSession session, Transaction transaction)
        {
            Session = session;
            Transaction = transaction;
        }

        #region IEsentTransaction Members

        public JET_SESID JetSesid
        {
            get { return Session.JetSesid; }
        }

        public IEsentSession Session { get; set; }

        public void Commit(CommitTransactionGrbit grbit)
        {
            Transaction.Commit(grbit);
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void Pulse(CommitTransactionGrbit grbit)
        {
            Transaction.Commit(grbit);
            Transaction = new Transaction(JetSesid);
        }

        public IEsentTable OpenTable(string name, OpenTableGrbit grbit)
        {
            return new EsentTable(this, new Table(JetSesid, Session.JetDbid, name, grbit));
        }

        public void Dispose()
        {
            Transaction.Dispose();
        }

        #endregion
    }
}