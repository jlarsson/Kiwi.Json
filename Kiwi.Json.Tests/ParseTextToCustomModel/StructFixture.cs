using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class StructFixture
    {
        public struct StructWithProperties
        {
            public string P1 { get; set; }
            public int P2 { get; set; }
        }

        public struct StructWithFields
        {
            public string F1;
            public int F2;
        }

        [Test]
        public void PropertiesAreIgnored()
        {
            var s = JsonConvert.Parse<StructWithProperties>(@"{""P1"":""hello"",""P2"":123}");

            s.P1.Should().Be.EqualTo(default(string));
            s.P2.Should().Be.EqualTo(default(int));
        }

        [Test]
        public void PublicFieldsAreParsed()
        {
            var s = JsonConvert.Parse<StructWithFields>(@"{""F1"":""hello"",""F2"":123}");

            s.F1.Should().Be.EqualTo("hello");
            s.F2.Should().Be.EqualTo(123);
        }
    }
}