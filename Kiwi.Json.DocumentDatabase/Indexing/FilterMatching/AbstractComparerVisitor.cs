using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing.FilterMatching
{
    public abstract class AbstractComparerVisitor: IJsonValueVisitor<bool>{

        public virtual bool VisitArray(IJsonArray value)
        {
            return value.Any(v => v.Visit(this));
        }

        public virtual bool VisitBool(IJsonBool value)
        {
            return false;
        }

        public virtual bool VisitDate(IJsonDate value)
        {
            return false;
        }

        public virtual bool VisitDouble(IJsonDouble value)
        {
            return false;
        }

        public virtual bool VisitInteger(IJsonInteger value)
        {
            return false;
        }

        public virtual bool VisitNull(IJsonNull value)
        {
            return false;
        }

        public virtual bool VisitObject(IJsonObject value)
        {
            return false;
        }

        public virtual bool VisitString(IJsonString value)
        {
            return false;
        }
    }
}