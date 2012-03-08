using System.Collections.Generic;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public class IndexDefinition
    {
        public IndexDefinition()
        {
            ExcludedValues = new HashSet<object>();
        }

        public string JsonPath { get; set; }
        public HashSet<object> ExcludedValues { get; set; }
    }
}