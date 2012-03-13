using System;
using System.IO;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentDatabase : IEsentDatabase
    {
        public EsentDatabase(string path)
        {
            DatabasePath = Path.GetFullPath(path);
        }

        public string DatabaseFolder { get; protected set; }

        #region IEsentDatabase Members

        public string DatabasePath { get; protected set; }

        public IEsentSession CreateSession(bool attachAndOpenDatabase)
        {
            var instance = default(IEsentInstanceHolder);
            var session = default(Session);
            try
            {
                instance = InstanceCache.GetInstance(DatabasePath);
                session = new Session(instance.Instance);
                var esentSession = new EsentSession(this, session, instance);
                if (attachAndOpenDatabase)
                {
                    esentSession.AttachDatabase(AttachDatabaseGrbit.None);
                    esentSession.OpenDatabase(null, OpenDatabaseGrbit.None);
                }
                return esentSession;
            }
            catch (Exception)
            {
                if (session != null)
                {
                    session.Dispose();
                }
                if (instance != null)
                {
                    instance.Dispose();
                }
                throw;
            }
        }

        #endregion
   }
}