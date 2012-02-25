using System;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class StructFixture
    {
        public struct TestStruct
        {
            public string F1;
            public int F2;
            public string P1 { get; set; }
            public int P2 { get; set; }
        }

        [Test]
        public void ParseFullStruct()
        {
            var s = JSON.Read<TestStruct>(@"{""F1"":""hello"",""F2"":123,""P1"":""world"",""P2"":456}");

            s.F1.Should().Be.EqualTo("hello");
            s.F2.Should().Be.EqualTo(123);
            s.P1.Should().Be.EqualTo("world");
            s.P2.Should().Be.EqualTo(456);
        }
    }
}