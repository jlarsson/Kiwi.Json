using System;
using System.Linq;
using Kiwi.Fluesent.Ddl;
using Microsoft.Isam.Esent.Interop;
using NUnit.Framework;

namespace Kiwi.Fluesent.Tests
{
    [TestFixture]
    public class ScratchFixture
    {
        public class KvRecord
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
        [Test]
        public void Test()
        {
            var dbdef = new DatabaseDefinition()
                .Table("KV")
                    .Column("Key").AsString().NotNull().Table
                    .Column("Value").AsText().NotNull().Table
                    .Index("IX_KV_Key").Asc("Key").Primary().Table
                .Database;



            var db = new EsentDatabase(@"c:\temp\fluesent\a.db");


            using (var instance = db.CreateInstance())
            {
                using (var session = instance.CreateSession(false))
                {
                    session.CreateDatabase(null, CreateDatabaseGrbit.OverwriteExisting);
                    using (var transaction = session.CreateTransaction())
                    {
                        dbdef.Create(transaction);

                        transaction.Commit();
                    }
                }
            }



            using (var instance = db.CreateInstance())
            {
                using (var session = instance.CreateSession())
                {
                    using (var transaction = session.CreateTransaction())
                    {
                        using (var table = transaction.OpenTable("KV"))
                        {
                            table.CreateInsertRecord()
                                .AddString("Key", "A")
                                .AddString("Value", "Some text")
                                .Insert();

                            table.CreateInsertRecord()
                                .AddString("Key", "B")
                                .AddString("Value", "Another text")
                                .Insert();
                            table.CreateInsertRecord()
                                .AddString("Key", "C")
                                .AddString("Value", "Text for C")
                                .Insert();
                        }
                        transaction.Commit();
                    }
                }
            }
            using (var instance = db.CreateInstance())
            {
                using (var session = instance.CreateSession())
                {
                    using (var transaction = session.CreateTransaction())
                    {
                        var stopwatch = new EsentStopwatch();
                        stopwatch.Start();
                        const int n = 100000;
                        using (var table = transaction.OpenTable("KV"))
                        {
                            foreach (var i in Enumerable.Range(0, n))
                            {
                                table.CreateInsertRecord()
                                    .AddString("Key", "key" + i)
                                    .AddString("Value", "akjsdhakhdkadhkajhd")
                                    .Insert();

                                if ((i % 1000) == 999)
                                {
                                    transaction.Pulse();
                                }
                            }
                        }
                        transaction.Commit();
                        stopwatch.Stop();
                        Console.Out.WriteLine("Inserted {0} records in {1}", n, stopwatch.Elapsed);
                    }
                }
            }
            using (var instance = db.CreateInstance())
            {
                using (var session = instance.CreateSession())
                {
                    using (var transaction = session.CreateTransaction())
                    {
                        using (var table = transaction.OpenTable("KV"))
                        {
                            var mapper = new RecordMapper<KvRecord>()
                                .String("Key", (o, s) => o.Key = s)
                                .String("Value", (o, s) => o.Value = s);

                            //var search = table.CreateSearch(mapper);

                            //var a = search.FindEq(
                            //    table.CreateKey().String("A")
                            //    )
                            //    .ToArray();
                            //var b = search.FindEq(
                            //    table.CreateKey().String("B")
                            //    )
                            //    .ToArray();
                        }
                    }
                }
            }

            using (var instance = db.CreateInstance())
            {
                using (var session = instance.CreateSession())
                {
                    session.AttachDatabase();
                    session.OpenDatabase();
                    using (var transaction = session.CreateTransaction())
                    {
                        using (var table = transaction.OpenTable("KV"))
                        {
                            var mapper = new RecordMapper<KvRecord>()
                                .String("Key", (o, s) => o.Key = s)
                                .String("Value", (o, s) => o.Value = s);

                            //var search = table.CreateSearch(mapper);

                            //foreach (var record in search.FindAll().Take(100))
                            //{
                            //    Console.Out.WriteLine("({0},{1})",record.Key,record.Value);
                            //}
                        }
                    }
                }
            }
        }
    }
}
