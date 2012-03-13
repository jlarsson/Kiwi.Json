using System.Linq;
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


            var indexes = coll.GetIndexes().ToList();


            coll.Put("A", new {B = "hello"});

            indexes = coll.GetIndexes().ToList();

            coll.Put("B", new {B = new[] {1, 2, 3}});

            coll.EnsureIndex(new IndexDefinition {JsonPath = JSON.ParseJsonPath("$.B")});
            coll.EnsureIndex(new IndexDefinition { JsonPath = JSON.ParseJsonPath("$.B") });

            indexes = coll.GetIndexes().ToList();

            var values = indexes[0].GetValues().ToList();

            var values2 = indexes[0].GetValues("B").ToList();

            coll.Put("C", new { B = new[] { 4,5,6 } });


            var o = coll.Find(new {B = 1});
        }
    }
}