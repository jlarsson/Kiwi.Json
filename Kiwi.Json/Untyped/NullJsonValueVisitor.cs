namespace Kiwi.Json.Untyped
{
    public class NullJsonValueVisitor<T>: IJsonValueVisitor<T> where T: class{
        public virtual T VisitArray(IJsonArray value)
        {
            return null;
        }

        public virtual T VisitBool(IJsonBool value)
        {
            return null;
        }

        public virtual T VisitDate(IJsonDate value)
        {
            return null;
        }

        public virtual T VisitDouble(IJsonDouble value)
        {
            return null;
        }

        public virtual T VisitInteger(IJsonInteger value)
        {
            return null;
        }

        public virtual T VisitNull(IJsonNull value)
        {
            return null;
        }

        public virtual T VisitObject(IJsonObject value)
        {
            return null;
        }

        public virtual T VisitString(IJsonString value)
        {
            return null;
        }
    }
}