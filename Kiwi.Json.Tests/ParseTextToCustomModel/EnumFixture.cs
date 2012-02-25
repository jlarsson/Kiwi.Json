using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
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
                JSON.Read<TestEnum>(@"""First"""),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                JSON.Read<TestEnum>(@"""Second"""),
                Is.EqualTo(TestEnum.Second));
        }

        [Test]
        public void ByValue()
        {
            Assert.That(
                JSON.Read<TestEnum>(@"11"),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                JSON.Read<TestEnum>(@"12"),
                Is.EqualTo(TestEnum.Second));
        }

    }
}