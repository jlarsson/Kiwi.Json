using System;
using System.Diagnostics;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonDateAndString : JsonDate, IJsonString
    {
        private readonly string _stringValue;

        public JsonDateAndString(DateTime value, string stringValue) : base(value)
        {
            _stringValue = stringValue;
        }

        #region IJsonString Members

        string IJsonString.Value
        {
            get { return _stringValue; }
        }

        #endregion
    }
}