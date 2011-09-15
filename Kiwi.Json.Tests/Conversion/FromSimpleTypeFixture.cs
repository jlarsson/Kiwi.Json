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
            JSON.FromObject(true)
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(true);

            JSON.FromObject(false)
                .Should().Be.InstanceOf<IJsonBool>()
                .And.Value.Value.Should().Be.EqualTo(false);
        }

        [Test]
        public void Dates()
        {
            var d = new DateTime(2011, 09, 15, 17, 08, 46, 233);
            JSON.FromObject(d)
                .Should().Be.InstanceOf<IJsonDate>()
                .And.Value.Value.Should().Be.EqualTo(d);
        }

        [Test]
        public void Floats()
        {
            JSON.FromObject(Math.PI)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(Math.PI);

            JSON.FromObject(3.14159f)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(3.14159f);

            JSON.FromObject(5.99m)
                .Should().Be.InstanceOf<IJsonDouble>()
                .And.Value.Value.Should().Be.EqualTo(5.99m);
        }

        [Test]
        public void Integers()
        {
            JSON.FromObject((byte) 1)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1);
            JSON.FromObject((sbyte)-11)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-11);

            JSON.FromObject((Int16)(-1234))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-1234);
            JSON.FromObject((UInt16)1234)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1234);

            JSON.FromObject((Int32)(-1234567))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-1234567);
            JSON.FromObject((UInt32)1234567)
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1234567);

            JSON.FromObject((Int64)(-123456789))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(-123456789);
            JSON.FromObject((UInt64)(123456789))
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(123456789);
        }

        [Test]
        public void Strings()
        {
            JSON.FromObject("hello Json")
                .Should().Be.InstanceOf<IJsonString>()
                .And.Value.Value.Should().Be.EqualTo("hello Json");
        }
    }
}