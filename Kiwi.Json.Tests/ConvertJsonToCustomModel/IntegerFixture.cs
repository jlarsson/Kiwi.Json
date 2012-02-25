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
                JSON.ToObject<int>(new JsonInteger(123)),
                Is.EqualTo(123)
                );
        }
    }
}