using System.Runtime.Serialization;
using NUnit.Framework;

namespace Kiwi.Json.Tests
{
    [TestFixture]
    public class DataContractFixture
    {
        [DataContract]
        public class Hider
        {
            [DataMember]
            public string Name { get; set; }

            [DataMember] public string Value;

            public string Hidden1 { get; set; }
            public string Hidden2;
        }

        [DataContract]
        public class Renamer
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "prop")] public string Value;

        }
        [Test]
        public void OnlyDataMembersAreSerialized()
        {
            Assert.That(
                JsonConvert.Write(new Hider{Name = "hello", Value = "world", Hidden1 = "secret1", Hidden2 = "secret2"}),

                Is.EqualTo(@"{""Name"":""hello"",""Value"":""world""}"));
        }

        [Test]
        public void DataMembersCanBeRenamed()
        {
            Assert.That(
                JsonConvert.Write(new Renamer { Name = "hello", Value = "world" }),
                Is.EqualTo(@"{""name"":""hello"",""prop"":""world""}"));
        }

    }
}