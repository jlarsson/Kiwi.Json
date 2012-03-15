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
            JsonConvert.Read("true")
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(true);

            JsonConvert.Read("false")
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(false);
        }
    }
}