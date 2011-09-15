using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kiwi.Json.Serialization;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kiwi.Json.PerformanceTests
{
    [TestFixture, Explicit]
    public class PerformanceFixture
    {
        public class Post
        {
            public string Author { get; set; }
            public string Text { get; set; }
            public List<string> Tags { get; set; }
        }

        private readonly Post _post = new Post
                                          {
                                              //Time = DateTime.Now,
                                              Author = DateTime.Now.ToString(),
                                              Text = string.Join(Environment.NewLine,
                                                                 Enumerable.Range(0, 10).Select(
                                                                     i => "This is a text line")),
                                              Tags = new List<string> {"tag1", "tag2", "tag3"}
                                          };

        private void Run<T, S>(string message, int count, T obj, Func<T, S> f)
        {
            Console.Out.WriteLine(message);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var n = Enumerable.Range(0, count).Select(i => f(obj)).Count();
            stopwatch.Stop();
            Console.Out.WriteLine("Elapsed: {0}", stopwatch.Elapsed);
            Console.Out.WriteLine("{0}/ms", n/stopwatch.Elapsed.TotalMilliseconds);
        }

        [Test]
        public void FromJson()
        {
            //var json = Conversion.JsonConverter.FromObject(_post).ToString();
            var json = JSON.FromObject(_post).PrettyPrint();

            const int warmUpCount = 1000;
            Run("Warming up Kiwi.Json", warmUpCount, json,
                s => JSON.ToObject<Post>(new JsonStringDeserializer(json).Parse()));
            Run("Warming up Kiwi.Json", warmUpCount, json, JsonConvert.DeserializeObject<Post>);

            const int runCount = 100000;
            Run("Kiwi.Json -> Native", runCount, json,
                s => JSON.ToObject<Post>(new JsonStringDeserializer(json).Parse()));
            Run("Newtonsoft.Json -> Native", runCount, json, JsonConvert.DeserializeObject<Post>);
        }

        [Test]
        public void Parse()
        {
            //var json = Conversion.JsonConverter.FromObject(_post).ToString();
            var json = JObject.FromObject(_post).ToString(Formatting.None);


            const int warmUpCount = 1000;
            Run("Warming up Kiwi.Json", warmUpCount, json, s => new JsonStringDeserializer(json).Parse());
            Run("Warming up Kiwi.Json", warmUpCount, json, JObject.Parse);

            const int runCount = 100000;
            Run("Kiwi.Json -> IJsonValue", runCount, json, s => new JsonStringDeserializer(json).Parse());
            Run("Newtonsoft.Json -> JObject", runCount, json, JObject.Parse);
        }

        [Test]
        public void ToJson()
        {
            const int warmUpCount = 1000;
            Run("Warming up Kiwi.Json", warmUpCount, _post, JSON.FromObject);
            Run("Warming up Newtonsoft.Json", warmUpCount, _post, JObject.FromObject);

            const int runCount = 1000000;
            Run("Native -> Json: Kiwi.Json", runCount, _post, JSON.FromObject);
            Run("Native -> Json: Newtonsoft.Json", runCount, _post, JObject.FromObject);
        }

        [Test]
        public void Write()
        {
            //var json = Conversion.JsonConverter.FromObject(_post).ToString();
            var jsonValue = JSON.FromObject(_post);
            var jobj = JObject.FromObject(_post);

            const int warmUpCount = 1000;
            Run("Warming up Kiwi.Json", warmUpCount, jsonValue, v => v.ToString());
            Run("Warming up Kiwi.Json", warmUpCount, jobj, v => v.ToString());

            const int runCount = 100000;
            Run("Kiwi.Json -> text", runCount, jsonValue, v => v.ToString());
            Run("Newtonsoft.Json -> text", runCount, jobj, v => v.ToString());
        }
    }
}