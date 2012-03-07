using System.Linq;
using Kiwi.Json.DocumentDatabase.Sqlite;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests
{
    [TestFixture]
    public class CollectionsAreCreatedWhenWrittenTo
    {
        [Test]
        public void Test()
        {
            var db = new MemoryDatabase();
            var coll = db.GetCollection("test collection");

            Assert.That(0, Is.EqualTo(db.Collections.Count()), "No collection should not exists since no writes are made");

            coll.Put("x",1);
            Assert.That(1, Is.EqualTo(db.Collections.Count()));

            Assert.That(new []{"test collection"}, Is.EqualTo(db.Collections.Select(c => c.Name)));
        }
    }
}