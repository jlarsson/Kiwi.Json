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
            var list = JSON.Read<IEnumerable<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void AbstractList()
        {
            var list = JSON.Read<IList<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Array()
        {
            var array = JSON.Read<int[]>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(array));
        }

        [Test]
        public void List()
        {
            var list = JSON.Read<List<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }

        [Test]
        public void Null()
        {
            var array = JSON.Read<int[]>(@"null");

            Assert.That(array, Is.Null);
        }

        [Test]
        public void SubclassedList()
        {
            var list = JSON.Read<SublassedList<int>>(@"[1,2,3]");

            Assert.That(new[] {1, 2, 3}, Is.EqualTo(list));
        }
    }
}