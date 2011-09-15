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
                .ConvertTo<bool>()
                .Should().Be.EqualTo(true);

            new JsonBool(false)
                .ConvertTo<bool>()
                .Should().Be.EqualTo(false);
        }

        [Test]
        public void Dates()
        {
            var d = new DateTime(2011, 09, 15, 17, 08, 46, 233);
            new JsonDate(d)
                .ConvertTo<DateTime>()
                .Should().Be.EqualTo(d);
        }

        [Test]
        public void Floats()
        {
            new JsonDouble(Math.PI)
                .ConvertTo<double>().Should().Be.EqualTo(Math.PI);
            new JsonDouble(3.14159)
                .ConvertTo<float>().Should().Be.EqualTo(3.14159f);
            new JsonDouble(5.99)
                .ConvertTo<decimal>().Should().Be.EqualTo(5.99m);
        }

        [Test]
        public void Integers()
        {
            new JsonInteger(1)
                .ConvertTo<byte>().Should().Be.EqualTo((byte) 1);
            new JsonInteger(-1)
                .ConvertTo<sbyte>().Should().Be.EqualTo((sbyte) -1);

            new JsonInteger(-1234)
                .ConvertTo<Int16>().Should().Be.EqualTo(-1234);
            new JsonInteger(1234)
                .ConvertTo<UInt16>().Should().Be.EqualTo(1234);

            new JsonInteger(-123456)
                .ConvertTo<Int32>().Should().Be.EqualTo(-123456);
            new JsonInteger(123456)
                .ConvertTo<UInt32>().Should().Be.EqualTo(123456);

            new JsonInteger(-123456789)
                .ConvertTo<Int64>().Should().Be.EqualTo(-123456789);
            new JsonInteger(123456789)
                .ConvertTo<UInt64>().Should().Be.EqualTo(123456789);
        }

        [Test]
        public void Strings()
        {
            const string s = "hello json";
            new JsonString(s)
                .ConvertTo<string>().Should().Be.EqualTo(s);
        }
    }
}