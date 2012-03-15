using System.Linq;
using NUnit.Framework;

namespace Kiwi.Json.Tests.JPath
{
    [TestFixture]
    public class GetJsonPathsFixture
    {
        [Test]
        public void WildcardPaths()
        {
            var obj = new
                          {
                              X = new
                                      {
                                          A = 1, 
                                          B = new object[]
                                                  {
                                                      new {Q = 1}
                                                  }
                                      }
                          };
            var expectedPaths = new[]
                                    {
                                        "$",
                                        "$.*",
                                        "$.X",
                                        "$.X.*",
                                        "$.X.A",
                                        "$.X.B",
                                        "$.X.B[*]",
                                        "$.X.B[*].*",
                                        "$.X.B[*].Q"
                                    };

            var jsonPaths = JsonConvert
                .ToJson(obj)
                .GetJsonPaths("$", true)
                .Distinct()
                .OrderBy(p => p)
                .ToArray();
            
            Assert.That(jsonPaths, Is.EqualTo(expectedPaths));
        }

        [Test]
        public void NonWildcardPaths()
        {
            var obj = new
            {
                X = new
                {
                    A = 1,
                    B = new object[]
                                                  {
                                                      new {Q = 1}
                                                  }
                }
            };
            var expectedPaths = new[]
                                    {
                                        "$",
                                        "$.X",
                                        "$.X.A",
                                        "$.X.B",
                                    };


            var jsonPaths = JsonConvert
                .ToJson(obj)
                .GetJsonPaths("$", false)
                .Distinct()
                .OrderBy(p => p)
                .ToArray();


            Assert.That(jsonPaths, Is.EqualTo(expectedPaths));

        }

    }
}