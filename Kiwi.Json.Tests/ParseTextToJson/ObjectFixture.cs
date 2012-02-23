using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture]
    public class ObjectFixture
    {
        [Test]
        public void Object()
        {
            var parsed = JSON.Parse(@"{""a"":1,""b"":2,""c"":""C"",""d"":[1,null,3]}");

            parsed.Should().Be.InstanceOf<IJsonObject>();

            var obj = parsed as IJsonObject;

            obj.Count.Should().Be.EqualTo(4);

            obj.ContainsKey("a").Should().Be.True();
            obj["a"].Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1);

            obj.ContainsKey("b").Should().Be.True();
            obj["b"].Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(2);

            obj.ContainsKey("c").Should().Be.True();
            obj["c"].Should().Be.InstanceOf<IJsonString>()
                .And.Value.Value.Should().Be.EqualTo("C");

            obj.ContainsKey("d").Should().Be.True();
            obj["d"].Should().Be.InstanceOf<IJsonArray>()
                .And.Value.Count.Should().Be.EqualTo(3);

        }
    }
}