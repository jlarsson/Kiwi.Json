using System;
using Kiwi.Json.Converters;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class AspNetDateTimeConverterFixture
    {
        [Test]
        public void Writing()
        {
            var dt = new DateTime(2012, 03, 25, 16, 01, 26, 494);

            var encoding = JsonConvert.Write(dt, new AspNetDateTimeConverter());

            Assert.That(encoding, Is.EqualTo(@"""\/Date(1332691286494)\/"""));
        }

        [Test]
        public void Parsing()
        {
            const string encoding = @"""\/Date(1332691286494)\/""";
            var expectedDate = new DateTime(2012, 03, 25, 16, 01, 26, 494);
            Assert.That(JsonConvert.Parse<DateTime>(encoding, new AspNetDateTimeConverter()), Is.EqualTo(expectedDate));
        }
        
    }
}