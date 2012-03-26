using System;
using System.Diagnostics;
using System.IO;
using JsonCompare;
using JsonCompare.Implementations;
using JsonCompare.Suites;
using Newtonsoft.Json;

namespace GenerateJsonCompareReport
{
    class Program
    {
        private static readonly IComplianceTestValuesProvider[] ComplianceTestValuesProviders = new IComplianceTestValuesProvider
            []
                                                                                           {
                                                                                               new SimpleValuesProvider(),
                                                                                               new CollectionValuesProvider(),
                                                                                               new DictionaryValuesProvider(), 
                                                                                           };

        static readonly IJsonImplementation[] Implementations = new IJsonImplementation[]
                                                                    {
                                                                        new KiwiJsonImlementation(), 
                                                                        new NewtonsoftJsonImplementations(),
                                                                        new SericestackTextImplementation(),
                                                                        new SystemJsonImplementation(),
                                                                        new JayrockJsonImplementation(), 
                                                                        new DanielCrennaJsonImplementation(), 
                                                                    };

        static void Main(string[] args)
        {

            var runner = new ComplianceTestRunner();

            var o = runner.Run(ComplianceTestValuesProviders, Implementations);

            Console.Out.WriteLine(JsonConvert.SerializeObject(o, Formatting.Indented));


            File.WriteAllText("report.js", JsonConvert.SerializeObject(o, Formatting.Indented));


            Process.Start("JsonReport.htm");

        }
    }
}
