using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class DoubleFixture
    {
        [Test]
        public void Double()
        {
            Assert.That(
                JsonConvert.Read<double>("123.45e67"),
                Is.EqualTo(123.45e67)
                );
        }
    }
}