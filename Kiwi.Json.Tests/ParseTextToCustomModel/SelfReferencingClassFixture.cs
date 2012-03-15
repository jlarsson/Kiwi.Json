using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class SelfReferencingClassFixture
    {
        public class Circular
        {
            public Circular Next { get; set; }
        }

        [Test]
        public void Test()
        {
            var c = JsonConvert.Read<Circular>("{\"Next\":{\"Next\":null}}");
        }
    }
}