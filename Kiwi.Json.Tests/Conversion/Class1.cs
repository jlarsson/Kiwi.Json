using System.Collections.Generic;
using Kiwi.Json.Conversion;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class Fixture
    {
        public class A
        {
            public string Text { get; set; }
        }

        private void VerifyFromJsonIs<TFromJson>(object value)
        {
            var fromJson = new DefaultFromJson().GetFromJson(value.GetType());
            Assert.That(fromJson, Is.TypeOf<TFromJson>());
        }

        [Test]
        public void ArrayFromJson()
        {
            VerifyFromJsonIs<FromJsonToArray<int>>(new int[0]);
            VerifyFromJsonIs<FromJsonToArray<A>>(new A[0]);
        }

        [Test]
        public void IListFromJson()
        {
            VerifyFromJsonIs<FromJsonToList<List<int>, int>>(new List<int>());
        }
    }
}