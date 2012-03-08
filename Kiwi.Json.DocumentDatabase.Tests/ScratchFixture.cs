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


            coll.Put("A", new { K = 1, B = "one" });
            db.Dump();

            coll.Put("B", new { K = 2, B = "two" });
            coll.Put("C", new { K = 2, B = "two" });
            db.Dump();

            var x = coll.Find(new { K = 2 });

            var a = coll.Find(new {B = "two"});


            
        }
    }
}