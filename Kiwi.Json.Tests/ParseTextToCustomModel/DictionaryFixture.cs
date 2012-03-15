using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class DictionaryFixture
    {
        [Test]
        public void Test()
        {
            //var t = typeof (IDictionary<string, int>);

            //var x = t.GetGenericTypeDefinition();

            //bool b = x == typeof (Dictionary<,>);

            var d = JsonConvert.Read<Dictionary<string, int>>(@"{""a"":1,""b"":2}");

            d.Should().Have.Count.EqualTo(2);

            d.OrderBy(kv => kv.Key).Select(kv => kv.Key).Should().Have.SameSequenceAs("a", "b");
            d.OrderBy(kv => kv.Key).Select(kv => kv.Value).Should().Have.SameSequenceAs(1,2);
        }
    }
}