using System.IO;
using Kiwi.Json.Conversion;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Incidents
{
    [TestFixture]
    public class JsonTextParserCanDetectEndOfStream
    {
        [Test]
        public void Test()
        {
            Assert.That(new JsonTextParser(new StringReader("")).EndOfInput(), Is.True);
        }
    }
}
