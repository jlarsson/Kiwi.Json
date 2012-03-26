using System.Collections;
using System.Collections.Generic;

namespace JsonCompare.Suites
{
    public class DictionaryValuesProvider: ComplianceTestValuesProviderBase
    {
        public class ClassWithTwoStringProperties
        {
            public string A { get; set; }
            public string B { get; set; }
        }

        private static readonly IComplianceTestValue[] Values = new[]
                                                                    {
                                                                        V(new Hashtable(){{"one",1},{"two","two"}}),
                                                                        V(new Dictionary<string,object>{{"one",1},{"two","two"}}),
                                                                        V<IDictionary<string,object>>(new Dictionary<string,object>{{"one",1},{"two","two"}}),

                                                                        V<IDictionary<string,string>>(new Dictionary<string,string>{{"one","one"},{"two","two"}}),
                                                                        V<IDictionary<string,ClassWithTwoStringProperties>>(new Dictionary<string,ClassWithTwoStringProperties>{{"one", new ClassWithTwoStringProperties(){A = "a"}},{"two",new ClassWithTwoStringProperties(){B = "b"}}}),
                                                                    };
        public override string Name
        {
            get { return "Dictionaries"; }
        }

        public override IEnumerable<IComplianceTestValue> TestValues
        {
            get { return Values; }
        }
    }
}