using System;
using System.Collections.Generic;
using System.Linq;
using JsonCompare.Implementations;
using JsonCompare.Utility;

namespace JsonCompare.Suites
{
    public class ComplianceTestRunner : IComplianceTestRunner
    {
        public object Run(
            IEnumerable<IComplianceTestValuesProvider> valuesProviders,
            IEnumerable<IJsonImplementation> implementations
            )
        {
            return from provider in valuesProviders
                   select new
                              {
                                  name = provider.Name,
                                  results = Run(provider.TestValues, implementations)
                              };


        }

        private IEnumerable<object> Run(IEnumerable<IComplianceTestValue> values, IEnumerable<IJsonImplementation> implementations)
        {
            foreach (var value in values)
            {
                yield return new
                                 {
                                     name = TypeUtility.GetTypeName(value.Type),
                                     results = Run(value, implementations)
                                 };
            }
        }

        private IEnumerable<object> Run(IComplianceTestValue value, IEnumerable<IJsonImplementation> implementations)
        {
            return from implementation in implementations select Run(value, implementation);
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