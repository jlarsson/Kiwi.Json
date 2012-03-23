using System;
using Kiwi.Json.Converters;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class UriConverterFixture
    {
        [Test]
        public void Test()
        {
            var uri = new Uri("http://www.github.com/jlarsson");

            var json = JsonConvert.Write(uri, new UriConverter());

            Assert.That(json, Is.EqualTo(@"""http://www.github.com/jlarsson"""), "Uri should be serialized as string");


            var uri2 = JsonConvert.Parse<Uri>(json, new UriConverter());

            Assert.That(uri, Is.EqualTo(uri2));
        }
    }
}