using System.Linq;
using Kiwi.Json.DocumentDatabase.Data;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests.Esent
{
    [TestFixture]
    public class IndexFixture: DatabaseTestFixtureBase
    {
        [Test]
        public void EmptyCollectionHasNoIndexes()
        {
            var coll = Db.GetCollection("Index");
            Assert.AreEqual(0, coll.GetIndexes().Count());
        }

        [Test]
        public void VerifyObjectIndex()
        {
            var coll = Db.GetCollection("Index");
            coll.Put("K", new {A = new object[] {1, 2, 3, "four"}, Ignore = "this value is not covered by index"});
            coll.EnsureIndex(new IndexDefinition { JsonPath = JsonConvert.ParseJsonPath("$.A") });

            var index = coll.GetIndexes().FirstOrDefault(ix => ix.JsonPath.Path == "$.A");
            Assert.NotNull(index);

            var indexValues = index.GetValues("K").Select(v => v.ToObject()).ToArray();

            Assert.That(indexValues, Is.EqualTo(new object[]{1,2,3,"four"}));
        }

        [Test]
        public void Test()
        {
            var coll = Db.GetCollection("Index");


            var indexes = coll.GetIndexes().ToList();


            coll.Put("A", new {B = "hello"});

            indexes = coll.GetIndexes().ToList();

            coll.Put("B", new {B = new[] {1, 2, 3}});

            coll.EnsureIndex(new IndexDefinition { JsonPath = JsonConvert.ParseJsonPath("$.B") });
            coll.EnsureIndex(new IndexDefinition { JsonPath = JsonConvert.ParseJsonPath("$.B") });

            indexes = coll.GetIndexes().ToList();

            var values = indexes[0].GetValues().ToList();

            var values2 = indexes[0].GetValues("B").ToList();

            coll.Put("C", new { B = new[] { 4,5,6 } });


            var o = coll.Find(new {B = 1});
        }
    }
}