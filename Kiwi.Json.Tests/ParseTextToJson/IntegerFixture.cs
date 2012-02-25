using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture]
    public class IntegerFixture
    {
        [Test]
        public void Integer()
        {
            JSON.Read("0")
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(0);

            JSON.Read("123")
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(123);

            JSON.Read("-123")
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-123);
        }
    }
}
