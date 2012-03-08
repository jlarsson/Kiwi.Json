using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class StringComparerVisitor : AbstractComparerVisitor
    {
        private readonly string _value;

        public StringComparerVisitor(string value)
        {
            _value = value;
        }

        public override bool VisitString(IJsonString value)
        {
            return string.Equals(_value, value.Value, StringComparison.OrdinalIgnoreCase);
        }
    }
}