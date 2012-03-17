using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture, Description("Test for converting JsonArrays to CLR collections")]
    public class ToObjectCollectionFixture
    {
        [Test]
        public void ObjectArray()
        {
            new JsonArray {new JsonInteger(1), new JsonDouble(Math.PI), new JsonString("json")}
                .ToObject<object[]>()
                .Should().Have.SameSequenceAs(1, Math.PI, "json");
        }

        [Test]
        public void ObjectList()
        {
            new JsonArray { new JsonInteger(1), new JsonDouble(Math.PI), new JsonString("json") }
                .ToObject<List<object>>()
                .Should().Have.SameSequenceAs(1, Math.PI, "json");
        }
        [Test]
        public void ArrayList()
        {
            var arrayList = new JsonArray {new JsonInteger(1), new JsonDouble(Math.PI), new JsonString("json")}.ToObject<ArrayList>();

            arrayList.Should().Not.Be.Null();

            arrayList.OfType<object>().Should().Have.SameSequenceAs(1, Math.PI, "json");
        }
    }
}