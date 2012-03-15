using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class DictionaryFixture
    {
        public class A
        {
            public int Value { get; set; }
        }
        [Test]
        public void Dictionary()
        {
            var parsed = JsonConvert.Read<IDictionary<string, A>>(@"{""a"":{""Value"":1},""b"":{""Value"":2}}");

            parsed.Count.Should().Be.EqualTo(2);

            (from kv in parsed orderby kv.Key select kv.Key).Should().Have.SameSequenceAs("a", "b");

            (from kv in parsed where kv.Value != null orderby kv.Key select kv.Value.Value).Should().Have.SameSequenceAs
                (1, 2);
        }
    }
}
