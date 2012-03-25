using System;
using System.IO;
using Kiwi.Json.Conversion;
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

            var writer =  new StringWriter();
            for(var i = 0; i < 100; ++i)
            {
                writer.WriteLine(JsonConvert.Write(new { A = i }));
            }
            var s = writer.ToString();
            var reader = new JsonStringParser(writer.ToString());

            while (!reader.EndOfInput())
            {

                Console.Out.WriteLine(
                    JsonConvert.Write(new { json = JsonConvert.Parse(reader) }));
            }

        }
    }
}
