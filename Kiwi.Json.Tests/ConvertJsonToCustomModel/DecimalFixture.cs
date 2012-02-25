using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class DecimalFixture
    {
        [Test]
        public void Decimal()
        {
            Assert.That(
                new JsonDouble(123.45e-1d).ToObject<decimal>(),
                Is.EqualTo(123.45e-1M)
                );
        }
    }
}