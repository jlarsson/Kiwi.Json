using System;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.ConvertJsonToCustomModel
{
    [TestFixture]
    public class GuidFixture
    {
        [Test]
        public void Guid()
        {
            Assert.That(
                new JsonString("24e1112a-b4c9-4b69-8a64-24bc4672e9fe").ToObject<Guid>(),
                Is.EqualTo(new Guid("24e1112a-b4c9-4b69-8a64-24bc4672e9fe"))
                );

            Assert.That(
                new JsonString("{24e1112a-b4c9-4b69-8a64-24bc4672e9fe}").ToObject<Guid>(),
                Is.EqualTo(new Guid("24e1112a-b4c9-4b69-8a64-24bc4672e9fe"))
                );
        }
    }
}