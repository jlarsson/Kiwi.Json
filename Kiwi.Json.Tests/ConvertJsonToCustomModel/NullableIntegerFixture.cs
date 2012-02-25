using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class NullableIntegerFixture
    {
        [Test]
        public void Integer()
        {
            Assert.That(
                new JsonInteger(123).ToObject<int?>(),
                Is.EqualTo(123)
                );
        }

        [Test]
        public void Null()
        {
            Assert.That(
                new JsonNull().ToObject<int?>().HasValue,
                Is.False
                );
        }
    }
}