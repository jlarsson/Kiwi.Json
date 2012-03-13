using System.Collections.Specialized;
using Common.Logging;
using Kiwi.Json.DocumentDatabase.Esent;
using Microsoft.Isam.Esent.Interop;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests.Esent
{
    public class DatabaseTestFixtureBase
    {
        protected IDocumentDatabase Db { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter(new NameValueCollection(){{"showDateTime","false"}});  
            var db = new EsentDocumentDatabase(@"test\test.db");
            db.CreateDatabase(CreateDatabaseGrbit.OverwriteExisting);
            Db = db;
        }

        [TearDown]
        public virtual void TearDown()
        {
        }
    }
}