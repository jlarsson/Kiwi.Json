using System;
using System.Collections.Generic;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class ToObjectCollectionFixture
    {
        [Test]
        public void ObjectArray()
        {
            var a = JSON.ToObject<object[]>(new JsonArray(){new JsonInteger(1), new JsonDouble(Math.PI), new JsonString("json")});

            CollectionAssert.AreEqual(
                new object[]{1,Math.PI,"json"},
                a);
        }
        [Test]
        public void ObjectList()
        {
            var a = JSON.ToObject<List<object>>(new JsonArray() { new JsonInteger(1), new JsonDouble(Math.PI), new JsonString("json") });

            CollectionAssert.AreEqual(
                new object[] { 1, Math.PI, "json" },
                a);
        }
    }
}