using System.Collections.Generic;

namespace JsonCompare.Suites
{
    public interface IComplianceTestValuesProvider
    {
        string Name { get; }
        IEnumerable<IComplianceTestValue> TestValues { get; }
    }
}