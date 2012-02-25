using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class NullableDoubleFixture
    {
        [Test]
        public void Null()
        {
            Assert.That(
                JSON.Read<double?>("null").HasValue,
                Is.False
                );
        }

        [Test]
        public void Double()
        {
            Assert.That(
                JSON.Read<double?>("123.45e67"),
                Is.EqualTo(123.45e67)
                );
        }
    }
}