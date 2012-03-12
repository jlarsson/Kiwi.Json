using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentSession : IEsentSession
    {
        protected IEsentInstance Instance;
        private readonly Session _session;

        public EsentSession(IEsentInstance instance, Session session)
        {
            Instance = instance;
            _session = session;
            JetDbid = JET_DBID.Nil;
        }

        #region IEsentSession Members

        public JET_DBID JetDbid { get; protected set; }

        public JET_SESID JetSesid
        {
            get { return _session.JetSesid; }
        }

        public void CreateDatabase(string connect, CreateDatabaseGrbit grbit)
        {
            if (JetDbid != JET_DBID.Nil)
            {
                throw new ApplicationException("Only one database may be opened in an EsentSession");
            }
            JET_DBID dbid;
            Api.JetCreateDatabase(_session, Instance.Database.Path, connect, out dbid, grbit);
            JetDbid = dbid;
        }

        public void OpenDatabase(string connect, OpenDatabaseGrbit grbit)
        {
            if (JetDbid != JET_DBID.Nil)
            {
                throw new ApplicationException("Only one database may be opened in an EsentSession");
            }
            JET_DBID dbid;
            Api.JetOpenDatabase(_session, Instance.Database.Path, connect, out dbid, grbit);
            JetDbid = dbid;
        }

        public void AttachDatabase(AttachDatabaseGrbit grbit)
        {
            Api.JetAttachDatabase(_session, Instance.Database.Path, grbit);
        }

        public IEsentTransaction CreateTransaction()
        {
            return new EsentTransaction(this, new Transaction(_session));
        }

        public void Dispose()
        {
            _session.Dispose();
        }

        #endregion
    }
}