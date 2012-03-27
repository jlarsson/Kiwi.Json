using System;
using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Tests.Theories.Collections
{
    public class ArrayFixture : EncodingFixtureBase
    {
        protected override Type DefaultExpectedJsonType
        {
            get { return typeof (IJsonArray); }
        }

        protected override IEnumerable<IEncodingExpectation> GetExpectations()
        {
            return new[]
                       {
                           Expectation(new[] {1, 2, 3, 4}, "[1,2,3,4]"),
                           Expectation(new object[] {1, "two", 3, null}, "[1,\"two\",3,null]")
                       };
        }
    }
}
