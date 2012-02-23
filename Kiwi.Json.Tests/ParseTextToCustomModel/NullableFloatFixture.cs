using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class NullableFloatFixture
    {
        [Test]
        public void Null()
        {
            Assert.That(
                JSON.ToObject<float?>("null").HasValue,
                Is.False
                );
        }

        [Test]
        public void Float()
        {
            Assert.That(
                JSON.ToObject<float?>("123.45e6"),
                Is.EqualTo(123.45e6f)
                );
        }
    }
}