using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Theories.SimpleTypes
{
    [TestFixture]
    public class IntegerFixture : EncodingFixtureBase
    {
        protected override Type DefaultExpectedJsonType
        {
            get { return typeof(IJsonInteger); }
        }

        protected override IEnumerable<IEncodingExpectation> GetExpectations()
        {
            return new[]
                       {
                           Expectation<byte>(0, "0"),
                           Expectation<byte>(123, "123"),
                           Expectation<byte?>(123, "123"),
                           Expectation<byte?>(null, "null").ShouldParseTo<IJsonNull>(),

                           Expectation<sbyte>(0, "0"),
                           Expectation<sbyte>(123, "123"),
                           Expectation<sbyte>(-123, "-123"),
                           Expectation<sbyte?>(123, "123"),
                           Expectation<sbyte?>(-123, "-123"),
                           Expectation<sbyte?>(null, "null").ShouldParseTo<IJsonNull>(),

                           Expectation<short>(0, "0"),
                           Expectation<short>(12345, "12345"),
                           Expectation<short>(-12345, "-12345"),
                           Expectation<short?>(12345, "12345"),
                           Expectation<short?>(-12345, "-12345"),
                           Expectation<short?>(null, "null").ShouldParseTo<IJsonNull>(),

                           Expectation<ushort>(0, "0"),
                           Expectation<ushort>(12345, "12345"),
                           Expectation<ushort?>(12345, "12345"),
                           Expectation<ushort?>(null, "null").ShouldParseTo<IJsonNull>(),


                           Expectation<int>(0, "0"),
                           Expectation<int>(123456, "123456"),
                           Expectation<int>(-123456, "-123456"),
                           Expectation<int?>(123456, "123456"),
                           Expectation<int?>(-123456, "-123456"),
                           Expectation<int?>(null, "null").ShouldParseTo<IJsonNull>(),

                           Expectation<uint>(0, "0"),
                           Expectation<uint>(123456, "123456"),
                           Expectation<uint?>(123456, "123456"),
                           Expectation<uint?>(null, "null").ShouldParseTo<IJsonNull>(),

                           Expectation<long>(0, "0"),
                           Expectation<long>(123456789012, "123456789012"),
                           Expectation<long>(-123456789012, "-123456789012"),
                           Expectation<long?>(123456789012, "123456789012"),
                           Expectation<long?>(-123456789012, "-123456789012"),
                           Expectation<long?>(null, "null").ShouldParseTo<IJsonNull>(),

                           Expectation<ulong>(0, "0"),
                           Expectation<ulong>(123456789012, "123456789012"),
                           Expectation<ulong?>(123456789012, "123456789012"),
                           Expectation<ulong?>(null, "null").ShouldParseTo<IJsonNull>(),
                       };
        }
    }
}
