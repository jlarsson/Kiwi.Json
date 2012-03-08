using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    internal class CreateFilterComparerVisitor: IJsonValueVisitor<IJsonValueVisitor<bool>>
    {
        public IJsonValueVisitor<bool> VisitArray(IJsonArray value)
        {
            return new ArrayComparerVisitor(value);
        }

        public IJsonValueVisitor<bool> VisitBool(IJsonBool value)
        {
            return new BoolComparerVisitor(value.Value);
        }

        public IJsonValueVisitor<bool> VisitDate(IJsonDate value)
        {
            return new DateComparerVisitor(value.Value);
        }

        public IJsonValueVisitor<bool> VisitDouble(IJsonDouble value)
        {
            return new DoubleComparerVisitor(value.Value);
        }

        public IJsonValueVisitor<bool> VisitInteger(IJsonInteger value)
        {
            return new IntegerComparerVisitor(value.Value);
        }

        public IJsonValueVisitor<bool> VisitNull(IJsonNull value)
        {
            return new NullComparerVisitor();
        }

        public IJsonValueVisitor<bool> VisitObject(IJsonObject value)
        {
            return new ObjectComparerVisitor(value);
        }

        public IJsonValueVisitor<bool> VisitString(IJsonString value)
        {
            return new StringComparerVisitor(value.Value);
        }
    }
}