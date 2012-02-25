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
                JSON.ToObject<float?>(new JsonNull()).HasValue,
                Is.False
                );
        }

        [Test]
        public void Float()
        {
            Assert.That(
                JSON.ToObject<float?>(new JsonDouble(123.45e6)),
                Is.EqualTo(123.45e6f)
                );
        }
    }
}