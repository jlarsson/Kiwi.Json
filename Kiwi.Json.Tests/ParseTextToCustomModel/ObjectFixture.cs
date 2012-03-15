using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class ObjectFixture
    {
        [Test]
        public void Dictionary()
        {
            var d = JsonConvert.Read<object>(@"{""a"":1,""b"":2}")
                .Should().Be.InstanceOf<IDictionary<string, object>>()
                .And.Value;

            d.OrderBy(kv => kv.Key).Select(kv => kv.Key).Should().Have.SameSequenceAs("a", "b");
            d.OrderBy(kv => kv.Key).Select(kv => kv.Value).Should().Have.SameSequenceAs(1, 2);
        }

        [Test]
        public void List()
        {
            var l = JsonConvert.Read<object>(@"[1,2,3.14,""four""]")
                .Should().Be.InstanceOf<IList<object>>()
                .And.Value.Should().Have.SameSequenceAs(new object[]{1,2,3.14,"four"});
        }

        [Test]
        public void Integer()
        {
            JsonConvert.Read<object>("123")
                .Should().Be.InstanceOf<int>()
                .And.Value.Should().Be.EqualTo(123);
        }

        [Test]
        public void Null()
        {
            JsonConvert.Read<object>("null")
                .Should().Be.Null();
        }

        [Test]
        public void True()
        {
            JsonConvert.Read<object>("true")
                .Should().Be.InstanceOf<bool>()
                .And.Value.Should().Be.EqualTo(true);
        }
        [Test]
        public void False()
        {
            JsonConvert.Read<object>("false")
                .Should().Be.InstanceOf<bool>()
                .And.Value.Should().Be.EqualTo(false);
        }

        [Test]
        public void Double()
        {
            JsonConvert.Read<object>("3.14")
                .Should().Be.InstanceOf<double>()
                .And.Value.Should().Be.EqualTo(3.14);
        }

    }
}