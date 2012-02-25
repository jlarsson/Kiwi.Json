using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture]
    public class DoubleFixture
    {
        [Test]
        public void Double()
        {
            JSON.Read("0.1")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(0.1);

            JSON.Read("1.23")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(1.23);
            JSON.Read("-1.23")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(-1.23);
            JSON.Read("1.23e45")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(1.23e45);
            JSON.Read("-1.23e45")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(-1.23e45);
            JSON.Read("-1.23e-45")
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(-1.23e-45);
        }
    }
}