using System;
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
        private struct TestStruct
        {
            public int A;
            public string B;
        }

        public class ReferencingClassA
        {
            public int V { get; set; }
            public ReferencingClassB B { get; set; }
        }
        public class ReferencingClassB
        {
            public int V { get; set; }
            public ReferencingClassA A { get; set; }
        }

        public TestCaseData Case<T>(T value, string expectedJson)
        {
            return new TestCaseData(typeof(T), value, expectedJson);
        }

        public IEnumerable<TestCaseData> TestData
        {
            get
            {
                // Enums
                yield return Case(TestEnum.First, "\"First\"");
                yield return Case(TestEnum.Second, "\"Second\"");
                yield return Case(TestEnum.Third, "\"Third\"");

                // Nullable enums
                yield return Case<TestEnum?>(TestEnum.First, "\"First\"");
                yield return Case<TestEnum?>(null, "null");

                // Integers
                yield return Case<sbyte>(123, "123");
                yield return Case<byte>(123, "123");
                yield return Case<short>(123, "123");
                yield return Case<ushort>(123, "123");
                yield return Case<int>(123, "123");
                yield return Case<uint>(123, "123");
                yield return Case<long>(123, "123");
                yield return Case<ulong>(123, "123");

                // Nullable integers
                yield return Case<sbyte?>(123, "123");
                yield return Case<byte?>(123, "123");
                yield return Case<short?>(123, "123");
                yield return Case<ushort?>(123, "123");
                yield return Case<int?>(123, "123");
                yield return Case<uint?>(123, "123");
                yield return Case<long?>(123, "123");
                yield return Case<ulong?>(123, "123");

                yield return Case<sbyte?>(null,"null");
                yield return Case<byte?>(null, "null");
                yield return Case<short?>(null, "null");
                yield return Case<ushort?>(null, "null");
                yield return Case<int?>(null, "null");
                yield return Case<uint?>(null, "null");
                yield return Case<long?>(null, "null");
                yield return Case<ulong?>(null, "null");

                // Floating point numerics
                yield return Case<double>(1.234e56, "1.234E+56");
                yield return Case<float>(1.234e5f, "123400");

                // Nullable floating point numerics
                yield return Case<double?>(1.234e56, "1.234E+56");
                yield return Case<double?>(null, "null");
                yield return Case<float?>(1.234e5f, "123400");
                yield return Case<float?>(null, "null");

                // Guids
                yield return Case(new Guid("625a814d5c88469d8402bfd698c3c66f"), "\"625a814d5c88469d8402bfd698c3c66f\"");
                // Nullable guids
                yield return Case<Guid?>(new Guid("625a814d5c88469d8402bfd698c3c66f"), "\"625a814d5c88469d8402bfd698c3c66f\"");
                yield return Case<Guid?>(null, "null");

                // Datetime
                yield return Case(new DateTime(2012, 03, 07, 20, 35, 54), @"""2012-03-07T20:35:54""");
                // Nullable DateTime
                yield return Case<DateTime?>(new DateTime(2012, 03, 07, 20, 35, 54), @"""2012-03-07T20:35:54""");
                yield return Case<DateTime?>(null, "null");
            

                // Strings and characters
                yield return Case('c', "\"c\"");
                yield return Case("hello", "\"hello\"");

                // Enumerables
                yield return Case(new object[] { 1, "world", null }, "[1,\"world\",null]");
                yield return Case(new[] { 1, 2, 3 }, "[1,2,3]");
                yield return Case<IEnumerable<int>>(Enumerable.Range(1, 3), "[1,2,3]");
                yield return Case(new List<int> { 1, 2, 3 }, "[1,2,3]");
                yield return Case(new HashSet<int> { 1, 2, 3 }, "[1,2,3]");
                yield return Case(new ArrayList { 1, 2, "three" }, "[1,2,\"three\"]");

                // Dictionaries
                yield return Case(new Dictionary<string, int> { { "A", 1 }, { "B", 2 } }, "{\"A\":1,\"B\":2}");
                yield return Case(new Hashtable { { 1, 1 }, { "two", 2 } }, "{\"1\":1,\"two\":2}");

                // Structs
                
                yield return Case(new TestStruct(){A=1,B="abc"}, "{\"A\":1,\"B\":\"abc\"}");
                // Nullable structs
                yield return Case<TestStruct?>(new TestStruct() { A = 1, B = "abc" }, "{\"A\":1,\"B\":\"abc\"}");
                yield return Case<TestStruct?>(null, "null");

                // Classes
                yield return Case(new {A = 1, B = "abc", C = Enumerable.Range(0, 3)}, "{\"A\":1,\"B\":\"abc\",\"C\":[0,1,2]}");
                // Classes with nullable members
                yield return Case(new { A = (int?)null, B = (int?)1 }, "{\"A\":null,\"B\":1}");

                // System.Object
                yield return Case(new object(), "{}");
                yield return Case(new[]{new  object(), null, new object()}, "[{},null,{}]");

                // Object types that depends on eachother
                var o = new ReferencingClassA()
                {
                    V = 1,
                    B = new ReferencingClassB()
                    {
                        V = 2,
                        A = new ReferencingClassA()
                        {
                            V = 3
                        }
                    }
                };
                yield return Case(o, "{\"V\":1,\"B\":{\"V\":2,\"A\":{\"V\":3,\"B\":null}}}");

            }
        }

        [TestCaseSource("TestData")]
        public void Write(Type type, object value, string expectedJson)
        {
            Assert.That(expectedJson, Is.EqualTo(JsonConvert.Write(value)), "Serialization failed for {0} of type {1}",
                        value, type);
        }
    }
}