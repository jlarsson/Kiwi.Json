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
            IJsonValue json = JSON.FromObject(new[] {1, 2, 3});
            var list = JSON.ToObject<IEnumerable<int>>(json);
            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void AbstractList()
        {
            IJsonValue json = JSON.FromObject(new[] {1, 2, 3});
            var list = JSON.ToObject<IList<int>>(json);
            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Array()
        {
            IJsonValue json = JSON.FromObject(new[] {1, 2, 3});
            var array = JSON.ToObject<int[]>(json);
            Assert.That(new[] {1, 2, 3}, Is.EqualTo(array));
        }

        [Test]
        public void List()
        {
            IJsonValue json = JSON.FromObject(new[] {1, 2, 3});
            var list = JSON.ToObject<List<int>>(json);

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Null()
        {
            var array = JSON.ToObject<int[]>(new JsonNull());

            Assert.That(array, Is.Null);
        }

        [Test]
        public void SubclassedList()
        {
            IJsonValue json = JSON.FromObject(new[] {1, 2, 3});
            var list = JSON.ToObject<SublassedList<int>>(json);

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }
    }
}