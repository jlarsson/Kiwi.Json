using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Common.Logging;
using Microsoft.Isam.Esent.Interop;
using Microsoft.Isam.Esent.Interop.Windows7;

namespace Kiwi.Fluesent
{
    public static class InstanceCache
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly object CacheLock = new object();

        private static readonly Dictionary<string, Registration> Registrations =
            new Dictionary<string, Registration>(StringComparer.OrdinalIgnoreCase);

        public static IEsentInstanceHolder GetInstance(string databasePath)
        {
            lock (CacheLock)
            {
                Registration registration;
                if (Registrations.TryGetValue(databasePath, out registration))
                {
                    var holder = new EsentInstanceHolder(registration);
                    ++registration.ReferenceCount;
                    return holder;
                }

                try
                {
                    registration = new Registration(){DatabasePath = databasePath};
                    Registrations.Add(databasePath, registration);
                    registration.Instance = CreateInstance(Path.GetDirectoryName(databasePath));
                    ++registration.ReferenceCount;

                    Log.TraceFormat("Attaching database instance {0}", registration.DatabasePath);
                    return new EsentInstanceHolder(registration);
                }
                catch (Exception e)
                {
                    if (registration != null)
                    {
                        if (registration.Instance != null)
                        {
                            registration.Instance.Dispose();
                        }
                    }
                    Registrations.Remove(databasePath);
                    throw;
                }
            }
        }

        private static void ReleaseInstance(Registration registration)
        {
            lock (CacheLock)
            {
                --registration.ReferenceCount;
                if (registration.ReferenceCount == 0)
                {
                    registration.Instance.Dispose();
                    Registrations.Remove(registration.DatabasePath);

                    Log.TraceFormat("Released database instance {0}", registration.DatabasePath);
                }
            }
        }

        private static Instance CreateInstance(string databaseFolder)
        {
            var instance = new Instance(Guid.NewGuid().ToString("n"));
            instance.Parameters.CircularLog = true;
            instance.Parameters.CleanupMismatchedLogFiles = true;
            instance.Parameters.CreatePathIfNotExist = true;
            instance.Parameters.AlternateDatabaseRecoveryDirectory = databaseFolder;
            instance.Parameters.LogFileDirectory = databaseFolder;
            instance.Parameters.SystemDirectory = databaseFolder;
            instance.Parameters.TempDirectory = databaseFolder;

            instance.Init(EsentVersion.SupportsWindows7Features
                              ? Windows7Grbits.ReplayIgnoreLostLogs
                              : InitGrbit.None);
            return instance;
        }

        #region Nested type: EsentInstanceHolder

        private class EsentInstanceHolder : IEsentInstanceHolder
        {
            private static readonly ILog Log = LogManager.GetCurrentClassLogger();

            private readonly Registration _registration;
            private bool _isDisposed;

            public EsentInstanceHolder(Registration registration)
            {
                _registration = registration;
            }

            #region IEsentInstanceHolder Members

            public Instance Instance { get { return _registration.Instance; } }

            public void Dispose()
            {
                if (_isDisposed)
                {
                    Log.ErrorFormat("Illegal multiple disposal of Esent instance for {0}", _registration.DatabasePath);
                }
                else
                {
                    _isDisposed = true;
                    ReleaseInstance(_registration);
                }
            }

            #endregion

            public IDisposable CreateWriteLock()
            {
                return _registration.CreateWriteLock();
            }
        }

        #endregion

        #region Nested type: Registration

        private class Registration
        {
            private readonly object _writeLock = new object();
            public string DatabasePath { get; set; }
            public int ReferenceCount { get; set; }
            public Instance Instance { get; set; }

            public IDisposable CreateWriteLock()
            {
                Log.TraceFormat("Acquiring writelock for {0}", DatabasePath);
                var writeLock = new ExitLock(_writeLock);
                Monitor.Enter(_writeLock);
                return writeLock;
            }

            private class ExitLock : IDisposable
            {
                private static readonly ILog Log = LogManager.GetCurrentClassLogger();
                
                private readonly object _writeLock;
                private bool _isDisposed;

                public ExitLock(object writeLock)
                {
                    _writeLock = writeLock;
                }

                public void Dispose()
                {
                    if (_isDisposed)
                    {
                        Log.Error("Illegal multiple disposal of writelock");
                    }
                    else
                    {
                        _isDisposed = true;
                        Monitor.Exit(_writeLock);
                    }
                }
            }
        }

        #endregion
    }
}