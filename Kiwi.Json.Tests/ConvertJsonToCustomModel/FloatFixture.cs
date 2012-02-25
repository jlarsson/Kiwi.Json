using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class FloatFixture
    {
        [Test]
        public void Float()
        {
            Assert.That(
                new JsonDouble(123.45e-1).ToObject<float>(),
                Is.EqualTo(123.45e-1f)
                );
        }
    }
}