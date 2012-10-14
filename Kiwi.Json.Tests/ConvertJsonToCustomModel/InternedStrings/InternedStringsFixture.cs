using Kiwi.Json.Converters;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel.InternedStrings
{
    [TestFixture]
    public class InternedStringsFixture
    {
        [Test]
        public void AssumeStringsAreNotInternedByDefault()
        {
            const string json = @"[""a"",""a"",""a""]";

            var strings = JsonConvert.Parse<string[]>(json);

            Assert.That(strings,Is.EquivalentTo(new []{"a","a","a"}));

            Assert.That(strings[0], Is.Not.SameAs(strings[1]));
            Assert.That(strings[0], Is.Not.SameAs(strings[2]));
            Assert.That(strings[1], Is.Not.SameAs(strings[2]));
        }

        [Test]
        public void InternedStringsAreAllSame()
        {
            const string json = @"[""a"",""a"",""a"",null]";

            var strings = JsonConvert.Parse<string[]>(json, new InterningStringConverter());

            Assert.That(strings, Is.EquivalentTo(new[] { "a", "a", "a",null }));

            Assert.That(strings[0], Is.SameAs(strings[1]));
            Assert.That(strings[0], Is.SameAs(strings[2]));

            Assert.That(strings[3], Is.Null);
        }
    }
}
