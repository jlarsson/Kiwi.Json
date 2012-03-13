using System.Collections.Generic;
using Kiwi.Json.JPath;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public class IndexDefinition
    {
        public IndexDefinition()
        {
            //ExcludedValues = new HashSet<object>();
        }

        public IJsonPath JsonPath { get; set; }
        //public HashSet<object> ExcludedValues { get; set; }
    }
}