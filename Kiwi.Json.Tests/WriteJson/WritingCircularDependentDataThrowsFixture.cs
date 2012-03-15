using Kiwi.Json.Conversion;
using NUnit.Framework;

namespace Kiwi.Json.Tests.WriteJson
{
    [TestFixture]
    public class WritingCircularDependentDataThrowsFixture
    {
        public class Circular
        {
            public Circular Next { get; set; }
        }

        [Test]
        public void WritingCircularDependentDataThrowsException()
        {
            var last = new Circular();

            var first = new Circular() {Next = new Circular() {Next = new Circular(){Next = last}}};
            last.Next = first;

            Assert.Throws<JsonSerializationException>(() => JsonConvert.Write(first));
        }
    }
}