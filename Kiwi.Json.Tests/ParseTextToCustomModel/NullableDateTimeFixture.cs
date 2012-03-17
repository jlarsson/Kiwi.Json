using System;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class NullableDateTimeFixture
    {
        [Test]
        public void Null()
        {
            Assert.That(
                JsonConvert.Parse<DateTime?>("null").HasValue,
                Is.False
                );
        }

        [Test]
        public void DateTime()
        {
            Assert.That(
                JsonConvert.Parse<DateTime?>(@"""2011-09-01 13:59:16Z"""),
                Is.EqualTo(new DateTime(2011, 09, 01, 13, 59, 16))
                );
        }
    }
}