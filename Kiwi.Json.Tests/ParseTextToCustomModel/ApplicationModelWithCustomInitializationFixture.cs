using System.Collections.Generic;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class ApplicationModelWithCustomInitializationFixture
    {
        public class A
        {
            public A()
            {
                L = new List<int>{1,2,3};
            }

            public List<int> L { get; set; }
        }

        [Test]
        public void Test()
        {
            var a = JsonConvert.Read<A>(@"{""L"":[4,5,6]}");

            a.Should().Not.Be.Null();

            a.L.Should().Have.SameSequenceAs(4, 5, 6);

        }
    }
}