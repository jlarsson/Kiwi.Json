using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentInstance : IEsentInstance
    {
        protected Instance Instance;

        public EsentInstance(IEsentDatabase database, Instance instance)
        {
            Database = database;
            Instance = instance;
        }

        #region IEsentInstance Members

        public void Dispose()
        {
            Instance.Dispose();
        }

        public IEsentDatabase Database { get; protected set; }

        public IEsentSession CreateSession(bool attachAndOpenDatabase = true)
        {
            var session = new EsentSession(this, new Session(Instance));
            try
            {
                if (attachAndOpenDatabase)
                {
                    session.AttachDatabase();
                    session.OpenDatabase();
                }
                return session;
            }
            catch (Exception)
            {
                session.Dispose();
                throw;
            }
        }

        #endregion
    }
}