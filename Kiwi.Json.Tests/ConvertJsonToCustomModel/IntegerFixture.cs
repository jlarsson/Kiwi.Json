using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class IntegerFixture
    {
        [Test]
        public void Integer()
        {
            Assert.That(
                new JsonInteger(123).ToObject<int>(),
                Is.EqualTo(123)
                );
        }
    }
}