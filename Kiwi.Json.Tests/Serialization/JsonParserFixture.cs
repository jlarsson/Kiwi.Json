using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.Serialization
{
    [TestFixture]
    public class JsonParserFixture
    {
        [Test]
        public void Date()
        {
            JSON.Parse(@"""\/Date(634504823560000000)\/""")
                .Should().Be.InstanceOf<IJsonDate>()
                .And.Value.Value.Should().Be.EqualTo(new DateTime(2011, 09, 01, 13, 59, 16));
        }

        [Test]
        public void Double()
        {
            JSON.Parse("0.1")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(0.1);

            JSON.Parse("1.23")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(1.23);
            JSON.Parse("-1.23")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(-1.23);
            JSON.Parse("1.23e45")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(1.23e45);
            JSON.Parse("-1.23e45")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(-1.23e45);
            JSON.Parse("-1.23e-45")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(-1.23e-45);
        }

        [Test]
        public void Integer()
        {
            JSON.Parse("0")
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(0);

            JSON.Parse("123")
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(123);

            JSON.Parse("-123")
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-123);
        }

        [Test]
        public void String()
        {
            JSON.Parse(@"""hello Json""")
                .Should().Be.InstanceOf<IJsonString>()
                .And.Value.Value.Should().Be.EqualTo("hello Json");

            JSON.Parse(@"""\r\n\t\f\""\u1234""")
                .Should().Be.InstanceOf<IJsonString>()
                .And.Value.Value.Should().Be.EqualTo("\r\n\t\f\"\x1234");
        }
    }
}