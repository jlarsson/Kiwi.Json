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
                new JsonNull().ToObject<decimal?>().HasValue,
                Is.False
                );
        }

        [Test]
        public void Decimal()
        {
            Assert.That(
                new JsonDouble(123.45e6).ToObject<decimal?>(),
                Is.EqualTo(123.45e6M)
                );
        }
    }
}