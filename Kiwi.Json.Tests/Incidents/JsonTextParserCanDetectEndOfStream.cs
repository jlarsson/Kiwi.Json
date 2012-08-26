using System.IO;
using Kiwi.Json.Conversion;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Incidents
{
    [TestFixture]
    public class JsonTextParserCanDetectEndOfStream
    {
        [Test]
        public void DetectEmpty()
        {
            var parser = new JsonTextParser(new StreamReader(new FileStream("a.txt", FileMode.OpenOrCreate, FileAccess.Read)));
            Assert.That(parser.EndOfInput(), Is.True);


            Assert.That(new JsonTextParser(new StringReader("")).EndOfInput(), Is.True);
        }

        [Test]
        public void DetectTailWhitespace()
        {
            Assert.That(new JsonTextParser(new StringReader("  \r\n  ")).EndOfInput(), Is.True);
        }

        [Test]
        public void DetectEmptyAfterRead()
        {
            var parser = new JsonTextParser(new StringReader("{}"));
            JsonConvert.Parse(parser);
            Assert.That(parser.EndOfInput(), Is.True);
        }

        [Test]
        public void DetectTailWhitespaceAfterRead()
        {
            var parser = new JsonTextParser(new StringReader("{}  \r\n\t  "));
            JsonConvert.Parse(parser);
            Assert.That(parser.EndOfInput(), Is.True);
        }

    }
}
