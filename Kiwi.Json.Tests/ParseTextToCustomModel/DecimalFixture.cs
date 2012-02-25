using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class DecimalFixture
    {
        [Test]
        public void Decimal()
        {
            Assert.That(
                JSON.Read<decimal>("123.45e-1"),
                Is.EqualTo(123.45e-1M)
                );
        }
    }
}