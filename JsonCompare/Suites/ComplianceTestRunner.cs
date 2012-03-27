using System;
using System.Linq;
using JsonCompare.Implementations;
using JsonCompare.Utility;

namespace JsonCompare.Suites
{
    public class ComplianceTestRunner : IComplianceTestRunner
    {
        public object Run(
            IComplianceTestValuesProvider[] valuesProviders,
            IJsonImplementation[] implementations
            )
        {
            return new
                       {
                           implementations = from implementation in implementations select implementation.Name,
                           cases = from valuesProvider in valuesProviders
                                   let testValues = valuesProvider.TestValues
                                   select new
                                              {
                                                  name = valuesProvider.Name,
                                                  tests = from value in testValues
                                                            select new
                                                                       {
                                                                           name = TypeUtility.GetTypeName(value.Type),
                                                                           results =
                                                                from implementation in implementations
                                                                select Run(value, implementation)
                                                                       }
                                              }
                       };
        }

        private object Run(IComplianceTestValue value, IJsonImplementation implementation)
        {
            try
            {
                var encoding = value.Write(implementation);
                var decoding = value.ParseSameType(implementation, encoding);
                var encoding2 = decoding.Write(implementation);

                if (encoding != encoding2)
                {
                    return new
                               {
                                   implementation = implementation.Name,
                                   success = false,
                                   message = "assymetric serialization/deserialization"
                               };
                }

                var referenceImplementation = new KiwiJsonImlementation();
                var referenceEncoding = value.Write(referenceImplementation);
                var referenceEncoding2 = decoding.Write(referenceImplementation);
                if (referenceEncoding != referenceEncoding2)
                {
                    return new
                    {
                        implementation = implementation.Name,
                        success = false,
                        message = "non-compliant deserialization"
                    };
                }

                return new
                           {
                               implementation = implementation.Name,
                               success = true,
                               message = ""
                           };

            }
            catch (Exception e)
            {
                return new
                           {
                               implementation = implementation.Name,
                               success = false,
                               message = e.Message
                           };
            }
        }
    }
}