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
            var list = JsonConvert.Read<IEnumerable<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void AbstractList()
        {
            var list = JsonConvert.Read<IList<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Array()
        {
            var array = JsonConvert.Read<int[]>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(array));
        }

        [Test]
        public void List()
        {
            var list = JsonConvert.Read<List<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Null()
        {
            var array = JsonConvert.Read<int[]>(@"null");

            Assert.That(array, Is.Null);
        }

        [Test]
        public void SubclassedList()
        {
            var list = JsonConvert.Read<SublassedList<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }
    }
}