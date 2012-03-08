using System.Collections.Generic;
using Kiwi.Json.DocumentDatabase.Indexing;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests
{
    [TestFixture]
    public class FilterStrategyFixture
    {
        TestCaseData Case(object filter, object value, bool expectedMatch)
        {
            return new TestCaseData(JSON.ToJson(filter), JSON.ToJson(value), expectedMatch);
        }

        IEnumerable<TestCaseData> TestCaseData
        {
            get
            {
                // Match simple values
                yield return Case(
                    2,
                    new[] { 1, 2, 3 },
                    true
                    );
                yield return Case(
                    new []{1,3},
                    new[] { 1, 2, 3 },
                    true
                    );
                yield return Case(
                    5,
                    new[] { 1, 2, 3 },
                    false
                    );
                yield return Case(
                    new[] { 5 },
                    new[] { 1, 2, 3 },
                    false
                    );


                // Match scalar in array
                yield return Case(
                    new {A = 2},
                    new {A = new []{1,2,3}},
                    true
                    );
                yield return Case(
                    new { A = 5 },
                    new { A = new[] { 1, 2, 3 } },
                    false
                    );

                // Match arrays with arrays
                yield return Case(
                    new { A = new[] { 2 } },
                    new { A = new[] { 1, 2, 3 } },
                    true
                    );
                yield return Case(
                    new { A = new[] { 1,3 } },
                    new { A = new[] { 1, 2, 3 } },
                    true
                    );
                yield return Case(
                    new { A = new[] { 5 } },
                    new { A = new[] { 1, 2, 3 } },
                    false
                    );

                // Match objects
                yield return Case(
                    new { A = new {B = 1} },
                    new { A = new { B = 1, X = 1, Y = 1 }, Q = 1 },
                    true
                    );
                yield return Case(
                    new { A = new { B = 2 } },
                    new { A = new { B = 1, X = 1, Y = 1 } },
                    false
                    );

                // Case insesitive strings
                yield return Case(
                    new { A = "VALUE" },
                    new { A = "value" },
                    true
                    );
                yield return Case(
                    new { A = "value" },
                    new { A = "VALUE" },
                    true
                    );
                yield return Case(
                    new { A = "value" },
                    new { A = "value" },
                    true
                    );
                yield return Case(
                    new { A = "value" },
                    new { A = "another value" },
                    false
                    );
            }
        }

        [TestCaseSource("TestCaseData")]
        public void Write(IJsonValue filter, IJsonValue value, bool expectedMatch)
        {
            Assert.That(expectedMatch, Is.EqualTo(new FilterStrategy().CreateFilter(filter).Matches(value)));
        }
    }
}