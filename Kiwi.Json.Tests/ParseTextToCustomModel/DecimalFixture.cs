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
                JsonConvert.Parse<decimal>("123.45e-1"),
                Is.EqualTo(123.45e-1M)
                );
        }
    }
}