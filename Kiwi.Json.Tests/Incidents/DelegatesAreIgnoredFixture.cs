using System;
using Kiwi.Json.Conversion;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Incidents
{
    [TestFixture]
    public class DelegatesAreIgnoredFixture
    {
        public class ClassWithEvents
        {
            public event Action Event;
        }
        public class ClassWithActions
        {
            public Action Action { get; set; }
        }

        [Test]
        public void EventsAreSilentlyIgnored()
        {
            var instance = new ClassWithEvents();
            instance.Event += () => {};

            var json = JsonConvert.Write(instance);

            Assert.That(json,Is.EqualTo("{}"));
        }

        [Test]
        public void DelegateTypesAreWrittenAsNull()
        {
            var instance = new ClassWithActions()
                               {
                                   Action = () => { }
                               };

            var json = JsonConvert.Write(instance);
            Assert.That(json,Is.EqualTo(@"{""Action"":null}"));
        }

        [Test]
        public void DelegateTypesAreIgnoredWhenParsed()
        {
            var json = @"{""Action"":""a value not assignale to action""}";
            var obj = JsonConvert.Parse<ClassWithActions>(json);
        }

    }
}