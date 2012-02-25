using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class EnumFixture
    {
        public enum TestEnum
        {
            First = 11,
            Second = 12
        }

        [Test]
        public void ByName()
        {
            Assert.That(
                new JsonString("First").ToObject<TestEnum>(),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                new JsonString("Second").ToObject<TestEnum>(),
                Is.EqualTo(TestEnum.Second));
        }

        [Test]
        public void ByValue()
        {
            Assert.That(
                new JsonInteger(11).ToObject<TestEnum>(),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                new JsonInteger(12).ToObject<TestEnum>(),
                Is.EqualTo(TestEnum.Second));
        }

    }
}