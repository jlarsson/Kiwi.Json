using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture]
    public class NullFixture
    {
        [Test]
        public void Object()
        {
            JsonConvert.Read("null")
                .Should().Be.InstanceOf<IJsonNull>();
        }
    }
}