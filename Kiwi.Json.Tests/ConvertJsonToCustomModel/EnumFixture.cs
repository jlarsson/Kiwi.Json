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
                JSON.ToObject<TestEnum>(new JsonString("First")),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                JSON.ToObject<TestEnum>(new JsonString("Second")),
                Is.EqualTo(TestEnum.Second));
        }

        [Test]
        public void ByValue()
        {
            Assert.That(
                JSON.ToObject<TestEnum>(new JsonInteger(11)),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                JSON.ToObject<TestEnum>(new JsonInteger(12)),
                Is.EqualTo(TestEnum.Second));
        }

    }
}