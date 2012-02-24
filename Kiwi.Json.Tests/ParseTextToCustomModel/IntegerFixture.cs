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
                JSON.ToObject<int>("123"),
                Is.EqualTo(123)
                );
        }
    }
}