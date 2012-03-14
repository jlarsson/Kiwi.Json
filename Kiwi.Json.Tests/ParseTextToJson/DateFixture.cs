using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture, Description("Since dates are represented as string in json, sometimes parsed dates have dual representation")]
    public class DatesAreAlsoStringsFixture
    {
        [Test]
        public void DateStringIsParsedToDualdateAndString()
        {
            // test as date
            JSON.Read(@"""2012-03-13T22:13:24""")
                .Should().Be.InstanceOf<IJsonDate>()
                .And.Value.Value.Should().Be.EqualTo(new DateTime(2012, 03, 13, 22, 13, 24));

            JSON.Read(@"""2012-03-13T22:13:24""")
                .Should().Be.InstanceOf<IJsonString>()
                .And.Value.Value.Should().Be.EqualTo(@"2012-03-13T22:13:24");
        }
    }

    [TestFixture]
    public class DateFixture
    {
        [Test]
        public void DateInSortableFormat()
        {
            JSON.Read(@"""2012-03-13T22:13:24""")
                .Should().Be.InstanceOf<IJsonDate>()
                .And.Value.Value.Should().Be.EqualTo(new DateTime(2012,03,13,22,13,24));
        }

        [Test]
        public void DateInUniversalSortableFormat()
        {
            JSON.Read(@"""2012-03-13 22:13:24Z""")
                .Should().Be.InstanceOf<IJsonDate>()
                .And.Value.Value.Should().Be.EqualTo(new DateTime(2012, 03, 13, 22, 13, 24));
        }

    }
}