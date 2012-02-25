using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class NullableDecimalFixture
    {
        [Test]
        public void Null()
        {
            Assert.That(
                JSON.ToObject<decimal?>(new JsonNull()).HasValue,
                Is.False
                );
        }

        [Test]
        public void Decimal()
        {
            Assert.That(
                JSON.ToObject<decimal?>(new JsonDouble(123.45e6)),
                Is.EqualTo(123.45e6M)
                );
        }
    }
}