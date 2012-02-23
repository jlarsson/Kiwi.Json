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
                JSON.ToObject<DateTime?>("null").HasValue,
                Is.False
                );
        }

        [Test]
        public void DateTime()
        {
            Assert.That(
                JSON.ToObject<DateTime?>(@"""\/Date(634504823560000000)\/"""),
                Is.EqualTo(new DateTime(2011, 09, 01, 13, 59, 16))
                );
        }
    }
}