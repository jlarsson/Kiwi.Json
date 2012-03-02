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
            var s = JSON.Write(new A(){a = 123});
            Console.Out.WriteLine(s);

            var t = JSON.Read<A>(s);
        }
    }
}
