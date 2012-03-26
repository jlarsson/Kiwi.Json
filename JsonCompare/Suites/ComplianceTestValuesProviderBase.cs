using System;
using System.Collections.Generic;

namespace JsonCompare.Suites
{
    public abstract class ComplianceTestValuesProviderBase : IComplianceTestValuesProvider
    {
        public abstract string Name { get; }
        public abstract IEnumerable<IComplianceTestValue> TestValues { get; }

        protected static IComplianceTestValue V<T>(T value)
        {
            return new TestValue<T>(value);
        }

        private class TestValue<T> : IComplianceTestValue
        {
            public TestValue(T value)
            {
                Value = value;
            }

            #region IComplianceTestValue Members

            public Type Type
            {
                get { return typeof (T); }
            }

            public T Value { get; private set; }
            public string Write(IJsonImplementation implementation)
            {
                return implementation.Write<T>(Value);
            }

            public IComplianceTestValue ParseSameType(IJsonImplementation implementation, string encoding)
            {
                return new TestValue<T>(implementation.Parse<T>(encoding));
            }

            #endregion
        }
    }
}