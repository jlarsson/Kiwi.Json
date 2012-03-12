using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentTransaction : IEsentTransaction
    {
        public EsentTransaction(IEsentSession session, Transaction transaction)
        {
            Session = session;
            Transaction = transaction;
        }

        #region IEsentTransaction Members

        public Transaction Transaction { get; protected set; }

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
            Transaction = new Transaction(Session.Session);
        }

        public IEsentTable OpenTable(string name, OpenTableGrbit grbit)
        {
            return new EsentTable(this, new Table(Session.Session, Session.JetDbid, name, grbit));
        }

        public void Dispose()
        {
            Transaction.Dispose();
        }

        #endregion
    }
}