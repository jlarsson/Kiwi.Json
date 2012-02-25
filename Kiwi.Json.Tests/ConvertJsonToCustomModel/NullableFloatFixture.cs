using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class NullableFloatFixture
    {
        [Test]
        public void Null()
        {
            Assert.That(
                new JsonNull().ToObject<float?>().HasValue,
                Is.False
                );
        }

        [Test]
        public void Float()
        {
            Assert.That(
                new JsonDouble(123.45e6).ToObject<float?>(),
                Is.EqualTo(123.45e6f)
                );
        }
    }
}