using System;
using Kiwi.Json.DocumentDatabase.Esent;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests.Esent
{
    [TestFixture, Explicit]
    public class ScratchFixture
    {
        [Test]
        public void Test()
        {
            const string dbPath = @"c:\temp\testdb\test.db";
            var db = new EsentDocumentDatabase(dbPath).AlwaysCreateNew();


            db.GetCollection("Test").Put("A",new {X = 1});

            db.GetCollection("Test").Put("B", new { X = 1 });


            var x = db.GetCollection("Test").Get<IJsonObject>("A");


            foreach (var collection in db.Collections)
            {
                Console.Out.WriteLine(collection.Name);
            }

        }
    }
}
