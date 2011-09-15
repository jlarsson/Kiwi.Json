using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class ToTypedCollectionFixture
    {
        public class D {
            public string P { get; set; }
        }
        [Test]
        public void Array()
        {
            var a = JSON.ToObject<D[]>(
                new JsonArray
                    {
                        new JsonObject { {"P", new JsonString("Hello")} },
                        new JsonObject { {"P", new JsonString("Json")} }
                    });

            var names = a.Select(d => d.P).ToArray();
            CollectionAssert.AreEqual(
                new[]{"Hello", "Json"},
                names);

        }

        [Test]
        public void Enumerable()
        {
            var a = JSON.ToObject<IEnumerable<D>>(
                new JsonArray(){
                                   new JsonObject { {"P", new JsonString("Hello")} },
                                   new JsonObject { {"P", new JsonString("Json")} }
                               });

            var names = a.Select(d => d.P).ToArray();
            CollectionAssert.AreEqual(
                new[] { "Hello", "Json" },
                names);
        }

        [Test]
        public void List()
        {
            var a = JSON.ToObject<List<D>>(
                new JsonArray(){
                                   new JsonObject { {"P", new JsonString("Hello")} },
                                   new JsonObject { {"P", new JsonString("Json")} }
                               });

            var names = a.Select(d => d.P).ToArray();
            CollectionAssert.AreEqual(
                new[] { "Hello", "Json" },
                names);

        }
    }
}