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
    }
}