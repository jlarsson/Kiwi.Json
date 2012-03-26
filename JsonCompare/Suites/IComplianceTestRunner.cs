using System.Collections.Generic;

namespace JsonCompare.Suites
{
    public interface IComplianceTestRunner
    {
        object Run(
            IEnumerable<IComplianceTestValuesProvider> valuesProviders,
            IEnumerable<IJsonImplementation> implementations
            );
    }
}