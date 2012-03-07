using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kiwi.Json.Tests.WriteJson
{
    [TestFixture]
    public class WriteFixture
    {
        private enum TestEnum
        {
            First,
            Second,
            Third
        }

        public IEnumerable<TestCaseData> TestData
        {
            get
            {
                // Enums
                yield return new TestCaseData(TestEnum.First, "\"First\"");
                yield return new TestCaseData(TestEnum.Second, "\"Second\"");
                yield return new TestCaseData(TestEnum.Third, "\"Third\"");

                // Integers
                yield return new TestCaseData((sbyte) 123, "123");
                yield return new TestCaseData((byte) 123, "123");
                yield return new TestCaseData((short) 123, "123");
                yield return new TestCaseData((ushort) 123, "123");
                yield return new TestCaseData(123, "123");
                yield return new TestCaseData((uint) 123, "123");
                yield return new TestCaseData((long) 123, "123");
                yield return new TestCaseData((ulong) 123, "123");

                // Strings and characters
                yield return new TestCaseData("c", "\"c\"");
                yield return new TestCaseData("hello", "\"hello\"");

                // Enumerables
                yield return new TestCaseData(new object[] {1, "world", null}, "[1,\"world\",null]");
                yield return new TestCaseData(new[] {1, 2, 3}, "[1,2,3]");
                yield return new TestCaseData(Enumerable.Range(1, 3), "[1,2,3]");
                yield return new TestCaseData(new List<int> {1, 2, 3}, "[1,2,3]");
                yield return new TestCaseData(new HashSet<int> {1, 2, 3}, "[1,2,3]");
                yield return new TestCaseData(new ArrayList { 1, 2, "three" }, "[1,2,\"three\"]");

                // Dictionaries
                yield return new TestCaseData(new Dictionary<string, int> {{"A", 1}, {"B", 2}}, "{\"A\":1,\"B\":2}");
                yield return new TestCaseData(new Hashtable {{1, 1}, {"two", 2}}, "{\"1\":1,\"two\":2}");
            }
        }

        [TestCaseSource("TestData")]
        public void Write(object @object, string expectedJson)
        {
            Assert.That(expectedJson, Is.EqualTo(JSON.Write(@object)), "Serialization failed for {0} of type {1}",
                        @object, @object.GetType());
        }
    }
}