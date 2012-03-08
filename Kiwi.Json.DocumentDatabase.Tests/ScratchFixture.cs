using System.Linq;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.DocumentDatabase.Sqlite;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests
{
    [TestFixture]
    public class ScratchFixture
    {
        [Test]
        public void GetQueryValues()
        {
            var p = JSON.ParseJsonPath("$.B[*]");

            var j = JSON.ToJson(new {K = 1, B = new []{"two"}});
            var x = p.Evaluate(j).ToArray();
        }

        [Test]
        public void Test()
        {
            var db = new MemoryDatabase();


            var coll = db.GetCollection("a");

            coll.EnsureIndex(new IndexDefinition()
            {
                JsonPath = "$.B"
            });

            coll.Put("A", new {K = 1, B = "one"});
            coll.Put("B", new { K = 2, B = "two" });


            coll.EnsureIndex(new IndexDefinition()
                                                  {
                                                      JsonPath = "$.B"
                                                  });
            coll.EnsureIndex(new IndexDefinition()
            {
                JsonPath = "$.B"
            });


            var a = coll.Find(new {B = "two"});


            a = coll.Find(new { K = 2 });
        }
    }
}