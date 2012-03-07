using Kiwi.Json.DocumentDatabase.Sqlite;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests
{
    [TestFixture]
    public class PutOverwritesExistingObject
    {
        [Test]
        public void Test()
        {
            var coll = new MemoryDatabase().GetCollection("test");
            coll.Put("samplekey", 1);
            coll.Put("samplekey", 2);

            Assert.That(2, Is.EqualTo(coll.Get<int>("samplekey")));
        }
    }
}