using System;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class FromSimpleTypeAndBackFixture
    {
        private void TestSimpleTypeToJsonAndBack<T>(T value)
        {
            var json = JSON.FromObject(value);
            var newValue = JSON.ToObject<T>(json);

            Assert.AreEqual(value, newValue);
        }

        [Test]
        public void Bool()
        {
            TestSimpleTypeToJsonAndBack(true);
            TestSimpleTypeToJsonAndBack(false);
        }

        [Test]
        public void Byte()
        {
            TestSimpleTypeToJsonAndBack<byte>(123);
        }

        [Test]
        public void Char()
        {
            TestSimpleTypeToJsonAndBack<UInt16>('@');
        }

        [Test]
        public void Date()
        {
            TestSimpleTypeToJsonAndBack(DateTime.Now);
        }

        [Test]
        public void Decimal()
        {
            TestSimpleTypeToJsonAndBack(123.456m);
        }

        [Test]
        public void Double()
        {
            TestSimpleTypeToJsonAndBack(Math.PI);
        }

        [Test]
        public void Int16()
        {
            TestSimpleTypeToJsonAndBack<Int16>(-1234);
        }

        [Test]
        public void Int32()
        {
            TestSimpleTypeToJsonAndBack(-123456);
        }

        [Test]
        public void Int64()
        {
            TestSimpleTypeToJsonAndBack(-123456789);
        }

        [Test]
        public void SByte()
        {
            TestSimpleTypeToJsonAndBack<sbyte>(-123);
        }

        [Test]
        public void Single()
        {
            TestSimpleTypeToJsonAndBack(1.23456f);
        }

        [Test]
        public void UInt32()
        {
            TestSimpleTypeToJsonAndBack<UInt32>(123456);
        }

        [Test]
        public void UInt64()
        {
            TestSimpleTypeToJsonAndBack<UInt32>(123456789);
        }
    }
}