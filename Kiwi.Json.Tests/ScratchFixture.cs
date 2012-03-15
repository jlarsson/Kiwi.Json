using System;
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
            var writer = new JsonStringWriter();


            for(var i = 0; i < 100; ++i)
            {
                JsonConvert.Write(new { A = i }, writer);
                writer.StringBuilder.AppendLine();
            }

            var reader = new JsonStringReader(writer.ToString());

            while (!reader.EndOfInput())
            {

                Console.Out.WriteLine(
                    JsonConvert.Write(new { json = JsonConvert.Read(reader) }));
            }

        }
    }
}
