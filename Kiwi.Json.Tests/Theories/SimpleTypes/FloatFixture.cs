using System;
using System.Collections.Generic;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Theories.SimpleTypes
{
    [TestFixture]
    public class FloatFixture : EncodingFixtureBase
    {
        protected override Type DefaultExpectedJsonType
        {
            get { return typeof (IJsonDouble); }
        }

        protected override IEnumerable<IEncodingExpectation> GetExpectations()
        {
            return new[]
                       {
                           Expectation<float>(0, "0").ShouldParseTo<IJsonInteger>(),
                           Expectation<float>(1.234e5f, "123400").ShouldParseTo<IJsonInteger>(),
                           Expectation<float?>(0, "0").ShouldParseTo<IJsonInteger>(),
                           Expectation<float?>(1.234e5f, "123400").ShouldParseTo<IJsonInteger>(),
                           Expectation<float?>(null, "null").ShouldParseTo<IJsonNull>(),


                           Expectation<double>(0, "0").ShouldParseTo<IJsonInteger>(),
                           Expectation<double>(1.234e56, "1.234E+56"),
                           Expectation<double?>(0, "0").ShouldParseTo<IJsonInteger>(),
                           Expectation<double?>(1.234e56, "1.234E+56"),
                           Expectation<double?>(null, "null").ShouldParseTo<IJsonNull>(),

                           Expectation<decimal>(0m, "0").ShouldParseTo<IJsonInteger>(),
                           Expectation<decimal>(1.234e5m, "123400").ShouldParseTo<IJsonInteger>(),
                           Expectation<decimal>(1.234m, "1.234"),
                           Expectation<decimal?>(1.234e5m, "123400").ShouldParseTo<IJsonInteger>(),
                           Expectation<decimal?>(1.234m, "1.234"),
                           Expectation<decimal?>(null, "null").ShouldParseTo<IJsonNull>()
                       };
        }
    }
}