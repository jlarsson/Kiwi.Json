using System.Collections.Generic;
using Kiwi.Json.DocumentDatabase.Indexing.FilterMatching;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public class FilterStrategy: IJsonFilterStrategy
    {
        public IConvertJsonValueToIndexNormalForm NormalFormConverter { get; set; }

        public FilterStrategy()
        {
            NormalFormConverter = new ConvertJsonValueToIndexNormalForm();
        }

        public IJsonFilter CreateFilter(IJsonValue filter)
        {
            return new JsonFilter(NormalFormConverter.Convert(filter), NormalFormConverter);
        }

        public IEnumerable<IJsonValue> GetFilterValues(IJsonValue value)
        {
            return value.Visit(new ExtractFilterValuesVisitor());
        }
    }
}