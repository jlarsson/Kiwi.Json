using System;
using System.IO;
using Common.Logging;
using Kiwi.Fluesent.Ddl;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentDatabase : IEsentDatabase
    {
        private static ILog Log = LogManager.GetCurrentClassLogger();

        private class CreateDatabaseParameters
        {
            public IDatabaseDefinition DatabaseDefinition { get; set; }
            public bool IsCreated { get; set; }

            public bool AlwaysCreate { get; set; }
        }

        private CreateDatabaseParameters _createDatabaseParameters;

        public EsentDatabase(string path)
        {
            DatabasePath = Path.GetFullPath(path);
        }

        public string DatabaseFolder { get; protected set; }

        #region IEsentDatabase Members

        public string DatabasePath { get; protected set; }

        public void SetCreateDatabaseOptions(IDatabaseDefinition databaseDefinition, bool alwaysCreate)
        {
            _createDatabaseParameters = new CreateDatabaseParameters()
                                            {
                                                IsCreated = false,
                                                AlwaysCreate = alwaysCreate,
                                                DatabaseDefinition = databaseDefinition
                                            };
        }

        public IEsentSession CreateSession(bool attachAndOpenDatabase)
        {
            CreateDatabaseIfNeeded();

            var session = default (IEsentSession);
            try
            {
                session = CreateSession();
                if (attachAndOpenDatabase)
                {
                    session.AttachDatabase(AttachDatabaseGrbit.None);
                    session.OpenDatabase(null, OpenDatabaseGrbit.None);
                }
                return session;
            }
            catch(Exception)
            {
                if (session != null)
                {
                    session.Dispose();
                }
                throw;
            }
        }

        private void CreateDatabaseIfNeeded()
        {
            var create = _createDatabaseParameters;
            if (create != null)
            {
                if (!create.IsCreated)
                {
                    using (var session = CreateSession())
                    {
                        session.LockWrites();

                        create = _createDatabaseParameters;

                        if ((create != null) && !create.IsCreated)
                        {
                            Log.InfoFormat("Creating database {0}", DatabasePath);
                            if (create.AlwaysCreate || !File.Exists(DatabasePath))
                            {
                                session.CreateDatabase(null,
                                                            create.AlwaysCreate
                                                                ? CreateDatabaseGrbit.OverwriteExisting
                                                                : CreateDatabaseGrbit.None);
                                using (var transaction = session.CreateTransaction())
                                {
                                    create.DatabaseDefinition.Create(transaction);
                                    transaction.Commit(CommitTransactionGrbit.None);
                                }
                            }
                            create.IsCreated = true;
                        }
                    }
                }
            }
        }

        private IEsentSession CreateSession()
        {
            var instance = default(IEsentInstanceHolder);
            var session = default(Session);
            try
            {
                instance = InstanceCache.GetInstance(DatabasePath);
                session = new Session(instance.Instance);
                var esentSession = new EsentSession(this, session, instance);

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