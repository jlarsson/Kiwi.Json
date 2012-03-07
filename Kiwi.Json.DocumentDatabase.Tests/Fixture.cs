using System;
using System.IO;
using System.Linq;
using Kiwi.Json.DocumentDatabase.Sqlite;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests
{
    [TestFixture]
    public class Fixture
    {
        [Test]
        public void T()
        {
            var list = from p in Directory.GetFiles(@"c:\windows\system32")
                       let fi = new FileInfo(p)
                       select new
                                             {
                                                 fi.Name,
                                                 fi.DirectoryName,
                                                 fi.Extension,
                                                 fi.FullName,
                                                 fi.IsReadOnly,
                                                 fi.LastAccessTime,
                                                 fi.LastWriteTime,
                                                 fi.Length
                                             };

            var db = new MemoryDatabase();


            foreach (var s in list)
            {
                db.GetCollection("files").Put(s.FullName, s);
                //Console.Out.WriteLine(s);
                
            }
        }
        [Test]
        public void Test()
        {
            var db = new MemoryDatabase();

            db.Dump();

            var collection = db.GetCollection("sample");

            collection.Put("A", new {X = 1});
            db.Dump();
            collection.Put("A", new { X = 1 });

            db.GetCollection("foo").Put("apa", new []{1,2,3,4});
            db.Dump();

            var x = collection.Get<IJsonValue>("A");

            var y = db.GetCollection("x").Get<IJsonValue>("A");
        }
    }
}