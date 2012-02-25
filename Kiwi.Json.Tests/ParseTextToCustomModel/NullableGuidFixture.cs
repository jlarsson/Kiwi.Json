using System;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ParseTextToCustomModel
{
    [TestFixture]
    public class NullableGuidFixture
    {
        [Test]
        public void Guid()
        {
            Assert.That(
                JSON.Read<Guid?>(@"""24e1112a-b4c9-4b69-8a64-24bc4672e9fe"""),
                Is.EqualTo(new Guid("24e1112a-b4c9-4b69-8a64-24bc4672e9fe"))
                );

            Assert.That(
                JSON.Read<Guid?>(@"""{24e1112a-b4c9-4b69-8a64-24bc4672e9fe}"""),
                Is.EqualTo(new Guid("24e1112a-b4c9-4b69-8a64-24bc4672e9fe"))
                );
        }

        [Test]
        public void Null()
        {
            Assert.That(
                JSON.Read<Guid?>("null").HasValue,
                Is.False
                );
        }
    }
}