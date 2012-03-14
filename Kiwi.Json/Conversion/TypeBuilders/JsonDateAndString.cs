using System;
using System.Diagnostics;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonDateAndString: JsonDate, IJsonString
    {
        private readonly string _stringValue;

        public JsonDateAndString(DateTime value, string stringValue): base(value)
        {
            _stringValue = stringValue;
        }

        string IJsonString.Value
        {
            get { return _stringValue; }
        }
    }
}