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
                JsonConvert.Parse<int?>("123"),
                Is.EqualTo(123)
                );
        }

        [Test]
        public void Null()
        {
            Assert.That(
                JsonConvert.Parse<int?>("null").HasValue,
                Is.False
                );
        }
    }
}