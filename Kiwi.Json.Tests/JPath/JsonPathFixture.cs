using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.JPath
{
    [TestFixture]
    public class JsonPathFixture
    {
        [Test]
        public void Test()
        {
            var json = JSON.FromObject(new {A = "a", B = new {X = 1, Y = 2}});

            json.JsonPathValues().Select(v => v.Path.Path).ToArray()
                .Should().Have.SameSequenceAs("A", "B.X", "B.Y");

            json.JsonPathValues().Select(v => v.Value.ToObject()).ToArray()
                .Should().Have.SameSequenceAs("a", (long)1, (long)2);
        }
    }
}