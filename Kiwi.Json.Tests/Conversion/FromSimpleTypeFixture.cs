using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class FromSimpleTypeFixture
    {
        [Test]
        public void Bool()
        {
            JsonConvert.ToJson(true)
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(true);

            JsonConvert.ToJson(false)
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(false);
        }

        [Test]
        public void Dates()
        {
            var d = new DateTime(2011, 09, 15, 17, 08, 46, 233);
            JsonConvert.ToJson(d)
                .Should().Be.InstanceOf<IJsonDate>()
                .And.Value.Value.Should().Be.EqualTo(d);
        }

        [Test]
        public void Floats()
        {
            JsonConvert.ToJson(Math.PI)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(Math.PI);

            JsonConvert.ToJson(3.14159f)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(3.14159f);

            JsonConvert.ToJson(5.99m)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(5.99m);
        }

        [Test]
        public void Integers()
        {
            JsonConvert.ToJson((byte) 1)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1);
            JsonConvert.ToJson((sbyte)-11)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-11);

            JsonConvert.ToJson((Int16)(-1234))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-1234);
            JsonConvert.ToJson((UInt16)1234)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1234);

            JsonConvert.ToJson((Int32)(-1234567))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-1234567);
            JsonConvert.ToJson((UInt32)1234567)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1234567);

            JsonConvert.ToJson((Int64)(-123456789))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-123456789);
            JsonConvert.ToJson((UInt64)(123456789))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(123456789);
        }

        [Test]
        public void Strings()
        {
            JsonConvert.ToJson("hello Json")
                .Should().Be.InstanceOf<IJsonString>()
                .And.Value.Value.Should().Be.EqualTo("hello Json");
        }
    }
}