using System.Linq;
using NUnit.Framework;

namespace Kiwi.Json.Tests.JPath
{
    [TestFixture]
    public class JPathFixture
    {
        [Test]
        public void Test()
        {
            var json = JSON.FromObject(new {A = "a", B = new {X = 1, Y = 2}});

            CollectionAssert.AreEqual(new[] {"A", "B.X", "B.Y"},
                                      json.JsonPathValues().Select(v => v.Path.Path).ToArray());
            CollectionAssert.AreEqual(new object[] {"a", 1, 2},
                                      json.JsonPathValues().Select(v => v.Value.ToObject()).ToArray());
        }
    }
}