using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class ApplicationModelFixture
    {
        private readonly IJsonObject SampleJsonObject = (IJsonObject)JsonConvert.Parse(
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
        public void ConvertFullObject()
        {
            var blog = SampleJsonObject.ToObject<Blog>();

            blog.Should().Not.Be.Null();

            blog.Title.Should().Be.EqualTo("Sample blog");
            blog.Content.Should().Be.EqualTo("Hello world");
            blog.Tags.Should().Have.SameSequenceAs(new[] {"test", "json"});
        }

        [Test]
        public void ConvertPartialObject1()
        {
            var blog = SampleJsonObject.ToObject<BlogContentOnly>();

            blog.Should().Not.Be.Null();

            blog.Content.Should().Be.EqualTo("Hello world");

        }

        [Test]
        public void ConvertPartialObject2()
        {
            var blog = SampleJsonObject.ToObject<BlogTagsOnly>();

            blog.Should().Not.Be.Null();
            blog.Tags.Should().Have.SameSequenceAs(new[] { "test", "json" });
        }
    }
}