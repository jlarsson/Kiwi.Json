using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture]
    public class BoolFixture
    {
        [Test]
        public void Bool()
        {
            JSON.Read("true")
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(true);

            JSON.Read("false")
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(false);
        }
    }
}