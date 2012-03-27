using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Theories.SimpleTypes
{
    [TestFixture]
    public class GuidFixture : EncodingFixtureBase
    {
        protected override Type DefaultExpectedJsonType
        {
            get { return typeof (IJsonString); }
        }

        protected override IEnumerable<IEncodingExpectation> GetExpectations()
        {
            return new[]
                       {
                           Expectation<Guid>(new Guid("{A47D15CD-2300-4F87-8357-3364468B828F}"), "\"a47d15cd23004f8783573364468b828f\""),
                           Expectation<Guid?>(new Guid("{A47D15CD-2300-4F87-8357-3364468B828F}"), "\"a47d15cd23004f8783573364468b828f\""),
                           Expectation<Guid?>(null,"null").ShouldParseTo<IJsonNull>()
                       };
        }
    }
}