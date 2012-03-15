using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Conversion;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class Fixture
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
            var d = JsonConvert.Read(@"{""A"":1}", new CustomDictionary(StringComparer.OrdinalIgnoreCase));
            Assert.That(new []{"A"}, Is.EqualTo(d.Keys.ToArray()));
            Assert.That(new[] { 1 }, Is.EqualTo(d.Values.ToArray()));
        }

        [Test]
        public void ClassesDontNeedDefaultConstructorIfInitialized()
        {
            //var d = JSON.Read(@"{""A"":1}", new CustomDictionary(StringComparer.OrdinalIgnoreCase));

            var json = @"{""Name"":""hello world""}";
            Assert.Throws<InvalidClassForDeserializationException>(() => JsonConvert.Read<A>(json));

            var a = JsonConvert.Read(json, new A("this will be overwritten"));
            Assert.That(a, Is.Not.Null);
            Assert.That(a.Name, Is.EqualTo("hello world"));
        }

        [Test]
        public void Test()
        {
            var d = JsonConvert.Read<Dictionary<string, object>>(@"{""A"":1}");
        }
    }

    [TestFixture]
    public class ApplicationModelFixture
    {
        private const string SampleJsonText =
            @"
                    {
                        ""Title"": ""Sample blog"",
                        ""Content"": ""Hello world"",
                        ""Tags"": [""test"",""json""]
                    }";

        public class Blog
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public string[] Tags { get; set; }
        }
        public class BlogContentOnly
        {
            public string Content { get; set; }
        }

        public class BlogTagsOnly
        {
            public string[] Tags { get; set; }

        }

        [Test]
        public void ParseFullObject()
        {
            var blog = JsonConvert.Read<Blog>(SampleJsonText);

            blog.Should().Not.Be.Null();

            blog.Title.Should().Be.EqualTo("Sample blog");
            blog.Content.Should().Be.EqualTo("Hello world");
            blog.Tags.Should().Have.SameSequenceAs(new[] {"test", "json"});
        }

        [Test]
        public void ParsePartialObject1()
        {
            var blog = JsonConvert.Read<BlogContentOnly>(SampleJsonText);

            blog.Should().Not.Be.Null();

            blog.Content.Should().Be.EqualTo("Hello world");

        }

        [Test]
        public void ParsePartialObject2()
        {
            var blog = JsonConvert.Read<BlogTagsOnly>(SampleJsonText);

            blog.Should().Not.Be.Null();
            blog.Tags.Should().Have.SameSequenceAs(new[] { "test", "json" });
        }

        [Test]
        public void ParseNull()
        {
            JsonConvert.Read<Blog>("null")
                .Should().Be.Null();
        }
    }
}