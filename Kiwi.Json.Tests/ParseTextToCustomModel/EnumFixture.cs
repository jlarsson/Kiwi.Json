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
                JsonConvert.Read<TestEnum>(@"""First"""),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                JsonConvert.Read<TestEnum>(@"""Second"""),
                Is.EqualTo(TestEnum.Second));
        }

        [Test]
        public void ByValue()
        {
            Assert.That(
                JsonConvert.Read<TestEnum>(@"11"),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                JsonConvert.Read<TestEnum>(@"12"),
                Is.EqualTo(TestEnum.Second));
        }

    }
}