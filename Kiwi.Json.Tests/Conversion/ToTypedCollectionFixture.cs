using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class ToTypedCollectionFixture
    {
        public class D
        {
            public string P { get; set; }
        }

        [Test]
        public void Array()
        {
            new JsonArray
                {
                    new JsonObject {{"P", new JsonString("Hello")}},
                    new JsonObject {{"P", new JsonString("Json")}}
                }
                .ConvertTo<D[]>()
                .Select(d => d.P)
                .Should().Have.SameSequenceAs("Hello", "Json")
                .And.Have.Count.EqualTo(2);
        }

        [Test]
        public void Enumerable()
        {
            new JsonArray
                {
                    new JsonObject {{"P", new JsonString("Hello")}},
                    new JsonObject {{"P", new JsonString("Json")}}
                }
                .ConvertTo<IEnumerable<D>>()
                .Select(d => d.P)
                .Should().Have.SameSequenceAs("Hello", "Json")
                .And.Have.Count.EqualTo(2);
        }

        [Test]
        public void List()
        {
            new JsonArray
                {
                    new JsonObject {{"P", new JsonString("Hello")}},
                    new JsonObject {{"P", new JsonString("Json")}}
                }
                .ConvertTo<List<D>>()
                .Select(d => d.P)
                .Should().Have.SameSequenceAs("Hello", "Json")
                .And.Have.Count.EqualTo(2);
        }
    }
}