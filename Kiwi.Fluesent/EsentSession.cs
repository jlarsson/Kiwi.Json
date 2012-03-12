using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentSession : IEsentSession
    {
        public EsentSession(IEsentInstance instance, Session session)
        {
            Instance = instance;
            Session = session;
            JetDbid = JET_DBID.Nil;
        }

        public IEsentInstance Instance { get; protected set; }

        #region IEsentSession Members

        public JET_SESID JetSesid
        {
            get { return Session; }
        }

        public Session Session { get; protected set; }

        public JET_DBID JetDbid { get; protected set; }

        public void CreateDatabase(string connect, CreateDatabaseGrbit grbit)
        {
            if (JetDbid != JET_DBID.Nil)
            {
                throw new ApplicationException("Only one database may be opened in an EsentSession");
            }
            JET_DBID dbid;
            Api.JetCreateDatabase(Session, Instance.Database.Path, connect, out dbid, grbit);
            JetDbid = dbid;
        }

        public void OpenDatabase(string connect, OpenDatabaseGrbit grbit)
        {
            if (JetDbid != JET_DBID.Nil)
            {
                throw new ApplicationException("Only one database may be opened in an EsentSession");
            }
            JET_DBID dbid;
            Api.JetOpenDatabase(Session, Instance.Database.Path, connect, out dbid, grbit);
            JetDbid = dbid;
        }

        public void AttachDatabase(AttachDatabaseGrbit grbit)
        {
            Api.JetAttachDatabase(Session, Instance.Database.Path, grbit);
        }

        public IEsentTransaction CreateTransaction()
        {
            return new EsentTransaction(this, new Transaction(Session));
        }

        public void Dispose()
        {
            Session.Dispose();
        }

        #endregion
    }
}