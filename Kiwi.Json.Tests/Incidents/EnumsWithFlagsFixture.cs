using System;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Incidents
{
    [TestFixture]
    public class EnumsWithFlagsFixture
    {
        [Flags]
        public enum FlaggedEnum
        {
            A = 0x01,
            B = 0x02,
            C = 0x04
        }
        [Test]
        public void Test()
        {
            const FlaggedEnum original = FlaggedEnum.A | FlaggedEnum.C;

            var jsonText = JsonConvert.Write(original);

            var parsed = JsonConvert.Parse<FlaggedEnum>(jsonText);

            Assert.That(parsed, Is.EqualTo(original));
        }
    }
}