using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class NullableDecimalFixture
    {
        [Test]
        public void Null()
        {
            Assert.That(
                JsonConvert.Parse<decimal?>("null").HasValue,
                Is.False
                );
        }

        [Test]
        public void Decimal()
        {
            Assert.That(
                JsonConvert.Parse<decimal?>("123.45e6"),
                Is.EqualTo(123.45e6M)
                );
        }
    }
}