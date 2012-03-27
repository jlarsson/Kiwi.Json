using System.Collections.Generic;

namespace JsonCompare.Suites
{
    public interface IComplianceTestRunner
    {
        object Run(
            IComplianceTestValuesProvider[] valuesProviders,
            IJsonImplementation[] implementations
            );
    }
}