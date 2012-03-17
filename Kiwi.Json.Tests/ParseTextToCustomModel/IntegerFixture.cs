using System;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class IntegerFixture
    {
        [Test]
        public void Integer()
        {
            Assert.That(
                JsonConvert.Parse<int>("123"),
                Is.EqualTo(123)
                );
        }
    }
}