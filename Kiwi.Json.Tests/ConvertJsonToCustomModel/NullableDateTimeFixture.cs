using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class NullableDateTimeFixture
    {
        [Test]
        public void Null()
        {
            Assert.That(
                new JsonNull().ToObject<DateTime?>().HasValue,
                Is.False
                );
        }

        [Test]
        public void DateTime()
        {
            var date = new DateTime(2011, 09, 01, 13, 59, 16);
            Assert.That(
                new JsonDate(date).ToObject<DateTime?>(),
                Is.EqualTo(date)
                );
        }
    }
}