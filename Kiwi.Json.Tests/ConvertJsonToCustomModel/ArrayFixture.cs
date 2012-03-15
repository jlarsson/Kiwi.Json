using System.Collections.Generic;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class ArrayFixture
    {
        public class SublassedList<T> : List<T>
        {
        }

        [Test]
        public void AbstractEnumerable()
        {
            var list = JsonConvert
                .ToJson(new[] { 1, 2, 3 })
                .ToObject<IEnumerable<int>>();
            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void AbstractList()
        {
            var list = JsonConvert
                .ToJson(new[] {1, 2, 3})
                .ToObject<IList<int>>();
            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Array()
        {
            var array = JsonConvert
                .ToJson(new[] {1, 2, 3})
                .ToObject<int[]>();
            Assert.That(new[] {1, 2, 3}, Is.EqualTo(array));
        }

        [Test]
        public void List()
        {
            var list = JsonConvert
                .ToJson(new[] {1, 2, 3})
                .ToObject<List<int>>();

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Null()
        {
            var array = new JsonNull().ToObject<int[]>();
            Assert.That(array, Is.Null);
        }

        [Test]
        public void SubclassedList()
        {
            var list = JsonConvert
                .ToJson(new[] {1, 2, 3})
                .ToObject<SublassedList<int>>();

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }
    }
}