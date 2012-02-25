using System;
using System.Collections.Generic;
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
                .Should().Have.SameSequenceAs((long)1, Math.PI, "json");
        }

        [Test]
        public void ObjectList()
        {
            new JsonArray { new JsonInteger(1), new JsonDouble(Math.PI), new JsonString("json") }
                .ToObject<List<object>>()
                .Should().Have.SameSequenceAs((long)1, Math.PI, "json");
        }
    }
}