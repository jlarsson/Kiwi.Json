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
                JSON.Read<decimal?>("null").HasValue,
                Is.False
                );
        }

        [Test]
        public void Decimal()
        {
            Assert.That(
                JSON.Read<decimal?>("123.45e6"),
                Is.EqualTo(123.45e6M)
                );
        }
    }
}