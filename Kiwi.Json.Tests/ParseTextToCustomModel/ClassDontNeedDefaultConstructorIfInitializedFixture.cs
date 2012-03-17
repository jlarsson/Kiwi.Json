using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class ClassDontNeedDefaultConstructorIfInitializedFixture
    {
        public class CustomDictionary : Dictionary<string, object>
        {
            public CustomDictionary(IEqualityComparer<string> comparer) : base(comparer)
            {
            }
        }

        public class A
        {
            public string Name { get; set; }

            public A(string name)
            {
                Name = name;
            }
        }

        [Test]
        public void DictionariesDontNeedDefaultConstructorIfInitialized()
        {
            var d = JsonConvert.Parse(@"{""A"":1}", new CustomDictionary(StringComparer.OrdinalIgnoreCase));
            Assert.That(new []{"A"}, Is.EqualTo(d.Keys.ToArray()));
            Assert.That(new[] { 1 }, Is.EqualTo(d.Values.ToArray()));
        }

        [Test]
        public void ClassesDontNeedDefaultConstructorIfInitialized()
        {
            //var d = JSON.Read(@"{""A"":1}", new CustomDictionary(StringComparer.OrdinalIgnoreCase));

            var json = @"{""Name"":""hello world""}";
            Assert.Throws<InvalidClassForDeserializationException>(() => JsonConvert.Parse<A>(json));

            var a = JsonConvert.Parse(json, new A("this will be overwritten"));
            Assert.That(a, Is.Not.Null);
            Assert.That(a.Name, Is.EqualTo("hello world"));
        }

        [Test]
        public void Test()
        {
            var d = JsonConvert.Parse<Dictionary<string, object>>(@"{""A"":1}");
        }
    }
}