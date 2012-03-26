using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JsonCompare.Suites
{
    public class CollectionValuesProvider : ComplianceTestValuesProviderBase
    {
        public class ClassWithTwoStringProperties
        {
            public string A { get; set; }
            public string B { get; set; }
        }
        private static readonly IComplianceTestValue[] Values = new[]
                                                                    {
                                                                        V(new object[]{1,2,3,4,"five", "six", "seven"}),
                                                                        V(new int[]{1,2,3,4,5,6}),
                                                                        //V(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } })


                                                                        V(new List<object>(){1,2,3,4,"five", "six", "seven"}),
                                                                        V(new List<int>{1,2,3,4,5,6}),

                                                                        V(new HashSet<object>(){1,2,3,4,"five", "six", "seven"}),
                                                                        V(new HashSet<int>{1,2,3,4,5,6}),

                                                                        V(Enumerable.Range(0,10)),

                                                                        V(new ArrayList(){1,2,3,4,"five","six"}),

                                                                        V(new[]{new ClassWithTwoStringProperties(){A ="a"}, new ClassWithTwoStringProperties(){B = "b"} }),
                                                                        V(new List<ClassWithTwoStringProperties>(){new ClassWithTwoStringProperties(){A ="a"}, new ClassWithTwoStringProperties(){B = "b"}})
                                                                    };
        public override string Name
        {
            get { return "Arrays"; }
        }

        public override IEnumerable<IComplianceTestValue> TestValues
        {
            get { return Values; }
        }
    }
}