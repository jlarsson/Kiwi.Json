using System;
using Microsoft.Isam.Esent.Interop;
using Microsoft.Isam.Esent.Interop.Windows7;

namespace Kiwi.Fluesent
{
    public class EsentDatabase : IEsentDatabase
    {
        private static object _sync = new object();
        private Instance _instance;
        private int _instanceCheckoutCount;

        public string Path { get; protected set; }
        public string DatabaseFolder { get; protected set; }

        public IEsentSession CreateSession(bool attachAndOpenDatabase)
        {
            var session = CheckoutSession();
            try
            {
                if (attachAndOpenDatabase)
                {
                    session.AttachDatabase(AttachDatabaseGrbit.None);
                    session.OpenDatabase(null, OpenDatabaseGrbit.None);
                }
                return session;
            }
            catch(Exception)
            {
                session.Dispose();
                throw;
            }
        }

        private EsentSession CheckoutSession()
        {
            lock (_sync)
            {
                if (_instance == null)
                {
                    ++_instanceCheckoutCount;
                    _instance = CheckoutInstance();
                }
                var session = default(EsentSession);
                try
                {
                    session = new EsentSession(this, new Session(_instance));
                    session.NotifyDisposed += s => CheckinInstance();
                }
                catch (Exception)
                {
                    CheckinInstance();
                    throw;
                }
                return session;
            }
        }

        private void CheckinInstance()
        {
            lock (_sync)
            {
                if (--_instanceCheckoutCount == 0)
                {
                    var instance = _instance;
                    _instance = null;
                    instance.Dispose();
                }
            }
        }

        private Instance CheckoutInstance()
        {
            var instance = new Instance(Guid.NewGuid().ToString("n"));
            var folder = System.IO.Path.GetDirectoryName(Path);
            instance.Parameters.CircularLog = true;
            instance.Parameters.CleanupMismatchedLogFiles = true;
            instance.Parameters.CreatePathIfNotExist = true;
            instance.Parameters.AlternateDatabaseRecoveryDirectory = folder;
            instance.Parameters.LogFileDirectory = folder;
            instance.Parameters.SystemDirectory = folder;
            instance.Parameters.TempDirectory = folder;

            instance.Init(Windows7Grbits.ReplayIgnoreLostLogs);

            return instance;
        }

        public EsentDatabase(string path)
        {
            Path = path;
            DatabaseFolder = System.IO.Path.GetDirectoryName(Path);
        }

/*
        public IEsentInstance CreateInstance(string name, string displayName, InitGrbit grbit)
        {
            var instance = new Instance(name, displayName);
            var folder = System.IO.Path.GetDirectoryName(Path);
            instance.Parameters.CircularLog = true;
            instance.Parameters.CleanupMismatchedLogFiles = true;
            instance.Parameters.CreatePathIfNotExist = true;
            instance.Parameters.AlternateDatabaseRecoveryDirectory = folder;
            instance.Parameters.LogFileDirectory = folder;
            instance.Parameters.SystemDirectory = folder;
            instance.Parameters.TempDirectory = folder;

            instance.Init(grbit);

            return new EsentInstance(this, instance);
        }
 */ 
    }
}