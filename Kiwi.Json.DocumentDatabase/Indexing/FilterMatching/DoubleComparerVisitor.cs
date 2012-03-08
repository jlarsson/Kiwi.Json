using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class DoubleComparerVisitor : AbstractComparerVisitor
    {
        private readonly double _value;

        public DoubleComparerVisitor(double value)
        {
            _value = value;
        }

        public override bool VisitDouble(IJsonDouble value)
        {
            return Math.Abs(_value - value.Value) < double.Epsilon;
        }
    }
}