using System;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class DateTimeFixture
    {
        [Test]
        public void DateTime()
        {
            Assert.That(
                JSON.Read<DateTime>(@"""\/Date(634504823560000000)\/"""),
                Is.EqualTo(new DateTime(2011, 09, 01, 13, 59, 16))
                );
        }
    }
}