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
                JSON.ToObject<DateTime?>(new JsonNull()).HasValue,
                Is.False
                );
        }

        [Test]
        public void DateTime()
        {
            var date = new DateTime(2011, 09, 01, 13, 59, 16);
            Assert.That(
                JSON.ToObject<DateTime?>(new JsonDate(date)),
                Is.EqualTo(date)
                );
        }
    }
}