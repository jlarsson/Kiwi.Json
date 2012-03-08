using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class DateComparerVisitor : AbstractComparerVisitor
    {
        private readonly DateTime _value;

        public DateComparerVisitor(DateTime value)
        {
            _value = value;
        }

        public override bool VisitDate(IJsonDate value)
        {
            return _value.Date == value.Value.Date;
        }
    }
}