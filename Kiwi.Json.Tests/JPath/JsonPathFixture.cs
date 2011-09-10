using Kiwi.Json.JPath;
using NUnit.Framework;

namespace Kiwi.Json.Tests.JPath
{
    [TestFixture]
    public class JsonPathFixture
    {
        [Test]
        public void Test()
        {
            var p = new JsonPath("A.B");

            var r = p.GetValue(JSON.FromObject(new {A = new {B = 1}}));

            Assert.AreEqual(1, r.ToObject());
        }
    }
}