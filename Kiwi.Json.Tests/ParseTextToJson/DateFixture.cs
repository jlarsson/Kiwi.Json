using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture]
    public class DateFixture
    {
        [Test]
        public void Date()
        {
            JSON.Read(@"""\/Date(634504823560000000)\/""")
                .Should().Be.InstanceOf<IJsonDate>()
                .And.Value.Value.Should().Be.EqualTo(new DateTime(2011, 09, 01, 13, 59, 16));
        }
    }
}