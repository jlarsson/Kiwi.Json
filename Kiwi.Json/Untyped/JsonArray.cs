using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kiwi.Json.JPath;
using Kiwi.Json.Conversion;

namespace Kiwi.Json.Untyped
{
    [DebuggerDisplay("{ToString()}")]
    public class JsonArray : List<IJsonValue>, IJsonArray
    {
        #region IJsonArray Members

        public IEnumerable<IJsonPathValue> JsonPathValues(string prefix)
        {
            yield return new JsonPathValue(prefix, this);
        }

        public void Write(IJsonWriter writer)
        {
            writer.WriteArrayStart();

            var index = 0;
            foreach (var item in this)
            {
                if (index++ > 0)
                {
                    writer.WriteArrayElementDelimiter();
                }
                item.Write(writer);
            }
            writer.WriteArrayEnd(index);
        }

        public object ToObject()
        {
            return this.Select(elem => elem.ToObject()).ToList();
        }

        public T Visit<T>(IJsonValueVisitor<T> visitor)
        {
            return visitor.VisitArray(this);
        }

        #endregion

        public override string ToString()
        {
            return this.PrettyPrint();
        }
    }
}