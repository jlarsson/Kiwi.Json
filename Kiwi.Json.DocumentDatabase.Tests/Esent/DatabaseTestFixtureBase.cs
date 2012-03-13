using System.Collections.Specialized;
using Common.Logging;
using Kiwi.Json.DocumentDatabase.Esent;
using Kiwi.Json.Untyped;
using Microsoft.Isam.Esent.Interop;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests.Esent
{
    public class DatabaseTestFixtureBase
    {
        private ICollectionSession _keepAliveSession;
        protected IDocumentDatabase Db { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter(new NameValueCollection(){{"showDateTime","false"}});
            var db = new EsentDocumentDatabase(@"test\test.db")
                .AlwaysCreateNew();
            //db.CreateDatabase(CreateDatabaseGrbit.OverwriteExisting);
            Db = db;

            // Create a session that keeps physical database connection up
            _keepAliveSession = db.GetCollection("a collection to keep connections to physical file alive").CreateSession();
            // We must do something to trigger it
            _keepAliveSession.Get<IJsonValue>("dummy key");


        }

        [TearDown]
        public virtual void TearDown()
        {
            _keepAliveSession.Dispose();
        }
    }
}