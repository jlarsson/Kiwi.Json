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
                JSON.ToObject<int?>(new JsonInteger(123)),
                Is.EqualTo(123)
                );
        }

        [Test]
        public void Null()
        {
            Assert.That(
                JSON.ToObject<int?>(new JsonNull()).HasValue,
                Is.False
                );
        }
    }
}