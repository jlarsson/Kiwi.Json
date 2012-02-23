using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class NullableIntegerFixture
    {
        [Test]
        public void Integer()
        {
            Assert.That(
                JSON.ToObject<int?>("123"),
                Is.EqualTo(123)
                );
        }

        [Test]
        public void Null()
        {
            Assert.That(
                JSON.ToObject<int?>("null").HasValue,
                Is.False
                );
        }
    }
}