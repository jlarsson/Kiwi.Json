using System;

namespace JsonCompare.Suites
{
    public interface IComplianceTestValue
    {
        Type Type { get; }
        string Write(IJsonImplementation implementation);
        IComplianceTestValue ParseSameType(IJsonImplementation implementation, string encoding);
    }
}