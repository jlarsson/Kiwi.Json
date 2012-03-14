using System;
using NUnit.Framework;

namespace Kiwi.Json.Tests
{
    [TestFixture]
    public class ScratchFixture
    {
        class A
        {
            public int? a;
            public int? b;
        }

        [Test]
        public void Test()
        {
            var a = JSON.ToJson(new {A = 1, Elems = new[] {1, 2, 3}, Name = "Hello", X = new {Y = 1}});
            var b = JSON.ToJson(new { A = 3, Elems = new[] { 4, 5 }, Q = 55, X = new { Y = 123 } });

            var x = a.MergeWith(b);





            var s = JSON.Write(new A(){a = 123});
            Console.Out.WriteLine(s);

            var t = JSON.Read<A>(s);
        }
    }
}
