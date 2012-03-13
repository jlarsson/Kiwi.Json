using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Isam.Esent.Interop;
using Microsoft.Isam.Esent.Interop.Windows7;

namespace Kiwi.Fluesent
{
    public static class InstanceCache
    {
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
                    var holder = new EsentInstanceHolder(databasePath) {Instance = registration.Instance};
                    ++registration.ReferenceCount;
                    return holder;
                }

                try
                {
                    registration = new Registration();
                    Registrations.Add(databasePath, registration);
                    registration.Instance = CreateInstance(Path.GetDirectoryName(databasePath));
                    ++registration.ReferenceCount;

                    return new EsentInstanceHolder(databasePath) {Instance = registration.Instance};
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

        private static void ReleaseInstance(string key)
        {
            lock (CacheLock)
            {
                Registration registration;
                if (!Registrations.TryGetValue(key, out registration))
                {
                    return;
                }
                --registration.ReferenceCount;
                if (registration.ReferenceCount == 0)
                {
                    registration.Instance.Dispose();
                }
                Registrations.Remove(key);
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
            private readonly string _key;
            private bool _isDisposed;

            public EsentInstanceHolder(string key)
            {
                _key = key;
            }

            #region IEsentInstanceHolder Members

            public Instance Instance { get; set; }

            public void Dispose()
            {
                Debug.Assert(!_isDisposed, "EsentInsstanceHolder is disposed more than once");
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    ReleaseInstance(_key);
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: Registration

        private class Registration
        {
            public int ReferenceCount { get; set; }
            public Instance Instance { get; set; }
        }

        #endregion
    }
}