using System;
using System.Linq;
using Kiwi.Json.DocumentDatabase.Esent;
using Kiwi.Json.Untyped;
using Microsoft.Isam.Esent.Interop;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests.Esent
{
    [TestFixture, Explicit]
    public class PerfScratchFixture : DatabaseTestFixtureBase
    {
        [Test]
        public void InsertAlot()
        {
            var objects = from _ in Enumerable.Range(0, 10000)
                          select new
                                     {
                                         Authorization = new {Roles = new[] {"Admin", "Owner"}},
                                         Content = new
                                                       {
                                                           Html = "a small html fragment on a webpage"
                                                       }

                                     };

            const string dbPath = @"c:\temp\testdb\test.db";
            var db = new EsentDocumentDatabase(dbPath).AlwaysCreateNew();
            //var db = Db;

            var coll = db.GetCollection("pages");

            // Make initial insert to create database
            db.GetCollection("init").Put("init","init");

            var sw = new EsentStopwatch();
            sw.Start();
            var n = 0;
            using (var session = coll.CreateSession())
            {
                const int batchSize = 1000;
                foreach (var o in objects)
                {
                    session.Put("object #" + n, JsonConvert.ToJson(o));
                    if ((n % batchSize) == (batchSize-1))
                    {
                        session.Pulse();
                    }
                    ++n;
                }
                session.Commit();
            }
            sw.Stop();
            Console.Out.WriteLine("Inserting {0} objects took {1}", n, sw.Elapsed);
            Console.Out.WriteLine("Inserting {0} objects took {1} ms/object", n, sw.Elapsed.TotalMilliseconds/n);
            Console.Out.WriteLine("Inserting {0} objects gave {1} objects/s", n, (double)n * 1000d / (double)sw.Elapsed.TotalMilliseconds);

            const int searchCount = 10000;
            var random = new Random();
            sw = new EsentStopwatch();
            sw.Start();
            for (var i = 0; i < searchCount; ++i )
            {
                var obj = coll.Get<IJsonObject>("object #" + random.Next(n));
            }
            sw.Stop();
            Console.Out.WriteLine("Finding {0} objects took {1}", searchCount, sw.Elapsed);
            Console.Out.WriteLine("Finding {0} objects took {1} ms/object", searchCount, sw.Elapsed.TotalMilliseconds/n);
            Console.Out.WriteLine("Finding {0} objects gave {1} objects/s", searchCount, (double)searchCount*1000d / (double)sw.Elapsed.TotalMilliseconds);

            sw = new EsentStopwatch();
            sw.Start();
            for (var i = 0; i < searchCount; ++i )
            {
                var obj = coll.Get<IJsonObject>("object %" + random.Next(n));
            }
            sw.Stop();
            Console.Out.WriteLine("Missing {0} objects took {1}", searchCount, sw.Elapsed);
            Console.Out.WriteLine("Missing {0} objects took {1} ms/object", searchCount, sw.Elapsed.TotalMilliseconds/n);
            Console.Out.WriteLine("Missing {0} objects gave {1} objects/s", searchCount, (double)searchCount * 1000d / (double)sw.Elapsed.TotalMilliseconds);
        }
    }
}