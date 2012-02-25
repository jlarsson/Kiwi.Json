using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class ToSimpleTypeFixture
    {
        [Test]
        public void Bool()
        {
            new JsonBool(true)
                .ToObject<bool>()
                .Should().Be.EqualTo(true);

            new JsonBool(false)
                .ToObject<bool>()
                .Should().Be.EqualTo(false);
        }

        [Test]
        public void Dates()
        {
            var d = new DateTime(2011, 09, 15, 17, 08, 46, 233);
            new JsonDate(d)
                .ToObject<DateTime>()
                .Should().Be.EqualTo(d);
        }

        [Test]
        public void Floats()
        {
            new JsonDouble(Math.PI)
                .ToObject<double>().Should().Be.EqualTo(Math.PI);
            new JsonDouble(3.14159)
                .ToObject<float>().Should().Be.EqualTo(3.14159f);
            new JsonDouble(5.99)
                .ToObject<decimal>().Should().Be.EqualTo(5.99m);
        }

        [Test]
        public void Integers()
        {
            new JsonInteger(1)
                .ToObject<byte>().Should().Be.EqualTo((byte)1);
            new JsonInteger(-1)
                .ToObject<sbyte>().Should().Be.EqualTo((sbyte)-1);

            new JsonInteger(-1234)
                .ToObject<Int16>().Should().Be.EqualTo(-1234);
            new JsonInteger(1234)
                .ToObject<UInt16>().Should().Be.EqualTo(1234);

            new JsonInteger(-123456)
                .ToObject<Int32>().Should().Be.EqualTo(-123456);
            new JsonInteger(123456)
                .ToObject<UInt32>().Should().Be.EqualTo(123456);

            new JsonInteger(-123456789)
                .ToObject<Int64>().Should().Be.EqualTo(-123456789);
            new JsonInteger(123456789)
                .ToObject<UInt64>().Should().Be.EqualTo(123456789);
        }

        [Test]
        public void Strings()
        {
            const string s = "hello json";
            new JsonString(s)
                .ToObject<string>().Should().Be.EqualTo(s);
        }
    }
}