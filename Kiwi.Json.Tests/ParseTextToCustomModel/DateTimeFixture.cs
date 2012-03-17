using System;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class DateTimeFixture
    {
        [Test]
        public void DateTimeInSortablePattern()
        {
            Assert.That(
                JsonConvert.Parse<DateTime>(@"""2011-09-01T13:59:16"""),
                Is.EqualTo(new DateTime(2011, 09, 01, 13, 59, 16))
                );
        }

        [Test]
        public void DateTimeInUniversalSortablePattern()
        {
            Assert.That(
                JsonConvert.Parse<DateTime>(@"""2011-09-01 13:59:16Z"""),
                Is.EqualTo(new DateTime(2011, 09, 01, 13, 59, 16))
                );
        }
    }
}