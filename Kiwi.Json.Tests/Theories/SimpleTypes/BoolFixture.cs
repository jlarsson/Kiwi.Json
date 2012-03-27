using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Theories.SimpleTypes
{
    [TestFixture]
    public class BoolFixture : EncodingFixtureBase
    {
        protected override Type DefaultExpectedJsonType
        {
            get { return typeof (IJsonBool); }
        }

        protected override IEnumerable<IEncodingExpectation> GetExpectations()
        {
            return new[]
                       {
                           Expectation<bool>(true, "true"),
                           Expectation<bool>(false, "false"),
                           Expectation<bool?>(true, "true"),
                           Expectation<bool?>(false, "false"),
                           Expectation<bool?>(null, "null").ShouldParseTo<IJsonNull>()
                       };
        }
    }
}