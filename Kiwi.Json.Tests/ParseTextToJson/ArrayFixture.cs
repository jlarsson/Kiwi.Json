using System.Collections.Generic;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.ParseTextToJson
{
    [TestFixture]
    public class ArrayFixture
    {
        [Test]
        public void Array()
        {
            var parsed = JsonConvert.Read(@"[1,2,3,""four""]");

            parsed.Should().Be.InstanceOf<IJsonArray>();

            var array = parsed as IJsonArray;

            array.ToObject()
                .Should().Be.InstanceOf<List<object>>();

            array.Count.Should().Be.EqualTo(4);


            array[0]
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(1);
            array[1]
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(2L);
            array[2]
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(3L);
            array[3]
                .Should().Be.InstanceOf<IJsonString>()
                .And.Value.Value.Should().Be.EqualTo("four");
        }
    }
}