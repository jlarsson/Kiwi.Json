using System.Collections.Generic;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
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
            var list = JsonConvert.Parse<IEnumerable<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void AbstractList()
        {
            var list = JsonConvert.Parse<IList<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Array()
        {
            var array = JsonConvert.Parse<int[]>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(array));
        }

        [Test]
        public void List()
        {
            var list = JsonConvert.Parse<List<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Null()
        {
            var array = JsonConvert.Parse<int[]>(@"null");

            Assert.That(array, Is.Null);
        }

        [Test]
        public void SubclassedList()
        {
            var list = JsonConvert.Parse<SublassedList<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }
    }
}