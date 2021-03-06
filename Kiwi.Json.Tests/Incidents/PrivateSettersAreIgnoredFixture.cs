using NUnit.Framework;

namespace Kiwi.Json.Tests.Incidents
{
    [TestFixture]
    public class PrivateSettersAreIgnoredFixture
    {
        public class A
        {
            public int P { get; private set; }
        }
        [Test]
        public void Test()
        {
            var a = JsonConvert.Parse<A>("{\"P\":1}");
            Assert.That(0, Is.EqualTo(a.P),"Property P with private setter should be ignored");
        }
    }
}