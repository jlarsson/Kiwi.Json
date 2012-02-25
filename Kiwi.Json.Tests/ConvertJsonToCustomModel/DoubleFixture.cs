using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class DoubleFixture
    {
        [Test]
        public void Double()
        {
            Assert.That(
                new JsonDouble(123.45e67).ToObject<double>(),
                Is.EqualTo(123.45e67)
                );
        }
    }
}