using System;
using System.Collections.Generic;

namespace JsonCompare.Suites
{
    public class SimpleValuesProvider : ComplianceTestValuesProviderBase
    {
        private static readonly IComplianceTestValue[] Values = new[]
                                                                     {
                                                                         V<bool>(true),
                                                                         V<bool?>(false),
                                                                         V<bool?>(null),

                                                                         V<byte>(123),
                                                                         V<byte?>(123),
                                                                         V<byte?>(null),
                                                                         V<sbyte>(123),
                                                                         V<sbyte>(-123),
                                                                         V<sbyte?>(123),
                                                                         V<sbyte?>(-123),
                                                                         V<sbyte?>(null),
                                                                         V<ushort>(12345),
                                                                         V<ushort?>(12345),
                                                                         V<ushort?>(null),
                                                                         V<short>(12345),
                                                                         V<short>(-12345),
                                                                         V<short?>(12345),
                                                                         V<short?>(-12345),
                                                                         V<short?>(null),
                                                                         V<uint>(123456),
                                                                         V<uint?>(123456),
                                                                         V<uint?>(null),
                                                                         V<int>(123456),
                                                                         V<int>(-123456),
                                                                         V<int?>(123456),
                                                                         V<int?>(-123456),
                                                                         V<int?>(null),
                                                                         V<ulong>(1234567890),
                                                                         V<ulong?>(1234567890),
                                                                         V<ulong?>(null),
                                                                         V<long>(1234567890),
                                                                         V<long>(-1234567890),
                                                                         V<long?>(1234567890),
                                                                         V<long?>(-1234567890),
                                                                         V<long?>(null),
                                                                         V(Math.PI),
                                                                         V<double?>(Math.PI),
                                                                         V<double?>(null),
                                                                         V(1.234e5f),
                                                                         V<float?>(1.234e5f),
                                                                         V<float?>(null),
                                                                         V(12345.67m),
                                                                         V<decimal?>(12345.67m),
                                                                         V<decimal?>(null),
                                                                         V<DateTime>(new DateTime(2012, 03, 26, 12, 53, 02, 456)),
                                                                         V<DateTime?>(new DateTime(2012, 03, 26, 12, 53, 02, 456)),
                                                                         V<DateTime?>(null)
                                                                     };

        #region IComplianceTestValuesProvider Members

        public override string Name
        {
            get { return "Simple value types"; }
        }

        public override IEnumerable<IComplianceTestValue> TestValues
        {
            get { return Values; }
        }

        #endregion

        #region Nested type: TestValue

        #endregion
    }
}