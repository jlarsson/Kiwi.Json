using System.Collections.Generic;
using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.FromJson;
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
            //var fromJson = JSON.ToObject<value.GetType()>
            //    new DefaultFromJson().GetFromJson(value.GetType());
            //Assert.That(fromJson, Is.TypeOf<TFromJson>());
            Assert.Fail("Weird test?");
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