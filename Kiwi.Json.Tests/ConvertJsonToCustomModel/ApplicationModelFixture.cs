using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class ApplicationModelFixture
    {
        private readonly IJsonObject SampleJsonObject = (IJsonObject)JSON.Parse(
            @"
                    {
                        ""Title"": ""Sample blog"",
                        ""Content"": ""Hello world"",
                        ""Tags"": [""test"",""json""]
                    }");

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
            var blog = JSON.ToObject<Blog>(SampleJsonObject);

            blog.Should().Not.Be.Null();

            blog.Title.Should().Be.EqualTo("Sample blog");
            blog.Content.Should().Be.EqualTo("Hello world");
            blog.Tags.Should().Have.SameSequenceAs(new[] {"test", "json"});
        }

        [Test]
        public void ParsePartialObject1()
        {
            var blog = JSON.ToObject<BlogContentOnly>(SampleJsonObject);

            blog.Should().Not.Be.Null();

            blog.Content.Should().Be.EqualTo("Hello world");

        }

        [Test]
        public void ParsePartialObject2()
        {
            var blog = JSON.ToObject<BlogTagsOnly>(SampleJsonObject);

            blog.Should().Not.Be.Null();
            blog.Tags.Should().Have.SameSequenceAs(new[] { "test", "json" });
        }
    }
}