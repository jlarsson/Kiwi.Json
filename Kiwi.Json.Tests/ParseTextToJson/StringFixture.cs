using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture]
    public class StringFixture
    {
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