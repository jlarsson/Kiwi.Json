using System.Linq;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class FromCollectionFixture
    {
        [Test]
        public void Enumerables()
        {
            var json = JSON.FromObject(Enumerable.Range(0, 10));


            json.Should().Be.InstanceOf<IJsonArray>();

            (json as IJsonArray).Should().Have.Count.EqualTo(10);

            (json as IJsonArray)
                .Select((jsonElem, i) => 
                    jsonElem.Should().Be.InstanceOf<IJsonInteger>()
                    .And.Value.Value.Should().Be.EqualTo(i)
                )
                .Should().Have.Count.EqualTo(10);
        }
    }
}