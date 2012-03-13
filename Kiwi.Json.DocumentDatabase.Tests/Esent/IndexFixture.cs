using Kiwi.Json.DocumentDatabase.Data;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests.Esent
{
    [TestFixture]
    public class IndexFixture: DatabaseTestFixtureBase
    {
        [Test]
        public void Test()
        {
            var coll = Db.GetCollection("Index");


            coll.Put("A", new {B = "hello"});
            coll.Put("B", new {B = new[] {1, 2, 3}});

            coll.EnsureIndex(new IndexDefinition {JsonPath = "$.B"});
            coll.EnsureIndex(new IndexDefinition { JsonPath = "$.B" });

            coll.Put("C", new { B = new[] { 4,5,6 } });


            var o = coll.Find(new {B = 1});
        }
    }
}