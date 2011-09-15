using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class FromSimpleTypeFixture
    {
        [Test]
        public void Bool()
        {
            Test<bool, IJsonBool>(true);
            Test<bool, IJsonBool>(false);
        }

        [Test]
        public void Integers()
        {
            Test<byte, IJsonInteger>(1);
            Test<sbyte, IJsonInteger>(-1);
            Test<Int16, IJsonInteger>(-1234);
            Test<UInt16, IJsonInteger>('@');
            Test<Int32,IJsonInteger>(-123456);
            Test<UInt32, IJsonInteger>(123456);
            Test<Int64, IJsonInteger>(-12345678);
            Test<UInt64, IJsonInteger>(12345678);
        }

        [Test]
        public void Floats()
        {
            Test<double,IJsonDouble>(Math.PI);
            Test<float, IJsonDouble>(3.14159f);
            Test<decimal, IJsonDouble>(5.99m);
        }

        [Test]
        public void Dates()
        {
            Test<DateTime, IJsonDate>(new DateTime(2011, 09,15,17,08,46,233));
        }

        [Test]
        public void Strings()
        {
            Test<string, IJsonString>("hello json");
        }

        private void Test<TClrType, TJsonInterface>(TClrType value)
        {
            var json = JSON.FromObject(value);
            Assert.That(json, Is.InstanceOf<TJsonInterface>());
            Assert.That(value, Is.EqualTo(json.ToObject()));
        }
    }
}