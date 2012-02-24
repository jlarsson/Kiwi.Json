using System;
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
                JSON.ToObject<TestEnum>(@"""First"""),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                JSON.ToObject<TestEnum>(@"""Second"""),
                Is.EqualTo(TestEnum.Second));
        }

        [Test]
        public void ByValue()
        {
            Assert.That(
                JSON.ToObject<TestEnum>(@"11"),
                Is.EqualTo(TestEnum.First));

            Assert.That(
                JSON.ToObject<TestEnum>(@"12"),
                Is.EqualTo(TestEnum.Second));
        }

    }
}