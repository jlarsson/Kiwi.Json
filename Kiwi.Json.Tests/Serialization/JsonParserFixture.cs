using System;
using Kiwi.Json.Serialization;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Serialization
{
    [TestFixture]
    public class JsonParserFixture
    {
        private static T Parse<T>(string text) where T : class, IJsonValue
        {
            var value = new JsonStringDeserializer(text).Parse();
            Assert.IsNotNull(value as T);

            return (T) value;
        }

        [Test]
        public void Date()
        {
            Assert.AreEqual(new DateTime(2011, 09, 01, 13, 59, 16),
                            Parse<IJsonDate>(@"""\/Date(634504823560000000)\/""").Value);
        }

        [Test]
        public void Double()
        {
            Assert.AreEqual(0.1, Parse<IJsonDouble>("0.1").Value);
            Assert.AreEqual(1.23, Parse<IJsonDouble>("1.23").Value);
            Assert.AreEqual(-1.23, Parse<IJsonDouble>("-1.23").Value);
            Assert.AreEqual(1.23e45, Parse<IJsonDouble>("1.23e45").Value);
            Assert.AreEqual(1.23E45, Parse<IJsonDouble>("1.23E45").Value);
            Assert.AreEqual(-1.23e-45, Parse<IJsonDouble>("-1.23e-45").Value);
        }

        [Test]
        public void Integer()
        {
            Assert.AreEqual(0, Parse<IJsonInteger>("0").Value);
            Assert.AreEqual(123, Parse<IJsonInteger>("123").Value);
            Assert.AreEqual(-123, Parse<IJsonInteger>("-123").Value);
        }

        [Test]
        public void String()
        {
            Assert.AreEqual("hello world", Parse<IJsonString>("\"hello world\"").Value);
            Assert.AreEqual("\r\n\t\f\"\x1234", Parse<IJsonString>(@"""\r\n\t\f\""\u1234""").Value);
        }
    }
}