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
            JSON.ToJson(true)
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(true);

            JSON.ToJson(false)
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(false);
        }

        [Test]
        public void Dates()
        {
            var d = new DateTime(2011, 09, 15, 17, 08, 46, 233);
            JSON.ToJson(d)
                .Should().Be.InstanceOf<IJsonDate>()
                .And.Value.Value.Should().Be.EqualTo(d);
        }

        [Test]
        public void Floats()
        {
            JSON.ToJson(Math.PI)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(Math.PI);

            JSON.ToJson(3.14159f)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(3.14159f);

            JSON.ToJson(5.99m)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(5.99m);
        }

        [Test]
        public void Integers()
        {
            JSON.ToJson((byte) 1)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1);
            JSON.ToJson((sbyte)-11)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-11);

            JSON.ToJson((Int16)(-1234))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-1234);
            JSON.ToJson((UInt16)1234)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1234);

            JSON.ToJson((Int32)(-1234567))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-1234567);
            JSON.ToJson((UInt32)1234567)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1234567);

            JSON.ToJson((Int64)(-123456789))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-123456789);
            JSON.ToJson((UInt64)(123456789))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(123456789);
        }

        [Test]
        public void Strings()
        {
            JSON.ToJson("hello Json")
                .Should().Be.InstanceOf<IJsonString>()
                .And.Value.Value.Should().Be.EqualTo("hello Json");
        }
    }
}