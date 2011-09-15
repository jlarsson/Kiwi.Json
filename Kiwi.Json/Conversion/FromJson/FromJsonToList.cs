using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.FromJson
{
    public class FromJsonToList<TList, TElem> : IFromJson where TList : IList<TElem>, new()
    {
        public FromJsonToList(IFromJson fromJsonConverter)
        {
            FromJsonConverter = fromJsonConverter;
        }

        public IFromJson FromJsonConverter { get; private set; }

        #region IFromJson Members

        public virtual object FromJson(Type nativeType, IJsonValue value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is IJsonNull)
            {
                return null;
            }

            var jsonArray = value as IJsonArray;
            if (jsonArray == null)
            {
                throw new InvalidCastException(string.Format("Cannot convert from {0} to {1}", value.GetType(),
                                                             typeof (TList)));
            }

            var list = new TList();
            var elems = jsonArray.Select(e => (TElem) FromJsonConverter.FromJson(typeof (TElem), e));
            if (list is List<TElem>)
            {
                (list as List<TElem>).AddRange(elems);
            }
            else
            {
                foreach (var elem in elems)
                {
                    list.Add(elem);
                }
            }
            return list;
        }

        #endregion
    }
}