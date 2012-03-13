using Kiwi.Json.DocumentDatabase.Esent;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests.Esent
{
    public class DatabaseTestFixtureBase
    {
        protected IDocumentDatabase Db { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            //LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter(new NameValueCollection(){{"showDateTime","false"}});
            Db = new EsentDocumentDatabase(@"test\test.db")
                .AlwaysCreateNew();
        }

        [TearDown]
        public virtual void TearDown()
        {
            EsentDocumentDatabase.Collect();
        }
    }
}