using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class ToSimpleTypeFixture
    {
        [Test]
        public void Bool()
        {
            Test<bool>(new JsonBool(true), true);
            Test<bool>(new JsonBool(false), false);
        }

        [Test]
        public void Integers()
        {
            Test<byte>(new JsonInteger(1), 1);
            Test<sbyte>(new JsonInteger(-1), -1);

            Test<Int16>(new JsonInteger(-1234), -1234);
            Test<UInt16>(new JsonInteger(1234), 1234);

            Test<Int32>(new JsonInteger(-123456), -123456);
            Test<UInt32>(new JsonInteger(123456), 123456);

            Test<Int64>(new JsonInteger(-123456789), -123456789);
            Test<UInt64>(new JsonInteger(123456789), 123456789);
        }

        [Test]
        public void Floats()
        {
            Test<double>(new JsonDouble(Math.PI), Math.PI);
            Test<float>(new JsonDouble(3.14159), 3.14159f);
            Test<decimal>(new JsonDouble(5.99), 5.99m);
        }

        [Test]
        public void Dates()
        {
            var d = new DateTime(2011, 09, 15, 17, 08, 46, 233);
            Test<DateTime>(new JsonDate(d), d);
        }

        [Test]
        public void Strings()
        {
            var s = "hello json";
            Test<string>(new JsonString(s),s );
        }

        private void Test<TClrType>(IJsonValue json, TClrType value)
        {
            var convertedValue = JSON.ToObject<TClrType>(json);
            Assert.That(value, Is.EqualTo(convertedValue));
        }
        
    }
}