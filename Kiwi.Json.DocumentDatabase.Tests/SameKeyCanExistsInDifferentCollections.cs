using Kiwi.Json.DocumentDatabase.Sqlite;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests
{
    [TestFixture]
    public class SameKeyCanExistsInDifferentCollections
    {
        [Test]
        public void Test()
        {
            var db = new MemoryDocumentDatabase();
            var c1 = db.GetCollection("c1");
            var c2 = db.GetCollection("c2");

            c1.Put("samplekey","X");
            c2.Put("samplekey", "Y");

            Assert.That("X", Is.EqualTo(c1.Get<string>("samplekey")));
            Assert.That("Y", Is.EqualTo(c2.Get<string>("samplekey")));
        }
    }
}