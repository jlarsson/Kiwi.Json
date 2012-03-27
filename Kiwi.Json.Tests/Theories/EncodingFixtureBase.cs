using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Theories
{
    [TestFixture]
    public abstract class EncodingFixtureBase
    {
        public IEnumerable<TestCaseData> WriteJsonData
        {
            get
            {
                return from expectation in GetExpectations()
                       select
                           new TestCaseData(expectation.ValueType, expectation.Value, expectation.ExpectedJsonEncoding);
            }
        }

        public IEnumerable<TestCaseData> ParseJsonData
        {
            get
            {
                return from expectation in GetExpectations()
                       select
                           new TestCaseData(expectation.ValueType, expectation.Value, expectation.ExpectedJsonEncoding);
            }
        }
        public IEnumerable<TestCaseData> ParseToUntypedJsonAndConvertData
        {
            get
            {
                return from expectation in GetExpectations()
                       select
                           new TestCaseData(expectation.JsonType ?? DefaultExpectedJsonType, expectation.ValueType, expectation.Value, expectation.ExpectedJsonEncoding);
            }
        }
        

        [TestCaseSource("WriteJsonData")]
        public void WriteJson(Type type, object value, string expectedJsonEncoding)
        {
            Assert.That(JsonConvert.Write(value), Is.EqualTo(expectedJsonEncoding));
        }

        [TestCaseSource("ParseJsonData")]
        public void ParseJson(Type type, object value, string expectedJsonEncoding)
        {
            Assert.That(JsonConvert.Parse(type, expectedJsonEncoding), Is.EqualTo(value));
        }


        [TestCaseSource("ParseToUntypedJsonAndConvertData")]
        public void ParseToUntypedJsonAndConvert(Type jsonType, Type valueType, object value, string expectedJsonEncoding)
        {
            var json = JsonConvert.Parse(expectedJsonEncoding);

            Assert.That(json, Is.InstanceOf(jsonType));
            Assert.That(json.ToObject(valueType), Is.EqualTo(value));
        }

        protected IEncodingExpectation Expectation<T>(T value, string expectedJsonEncoding)
        {
            return new EncodingExpectation<T>(value, expectedJsonEncoding);
        }

        protected abstract Type DefaultExpectedJsonType { get; }
        protected abstract IEnumerable<IEncodingExpectation> GetExpectations();

        #region Nested type: EncodingExpectation

        public class EncodingExpectation<T> : IEncodingExpectation
        {
            public EncodingExpectation(object value, string expectedJsonEncoding)
            {
                Value = value;
                ExpectedJsonEncoding = expectedJsonEncoding;
            }

            #region IEncodingExpectation Members

            public Type JsonType { get; private set; }

            public object Value { get; set; }
            public string ExpectedJsonEncoding { get; set; }
            public IEncodingExpectation ShouldParseTo<TJson>() where TJson : IJsonValue
            {
                JsonType = typeof(TJson);
                return this;
            }

            public Type ValueType
            {
                get { return typeof (T); }
            }

            #endregion
        }

        #endregion
    }
}