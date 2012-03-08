using Kiwi.Json.DocumentDatabase.Indexing.FilterMatching;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Indexing
{
    public class JsonFilter : IJsonFilter
    {
        private readonly IJsonValue _filter;
        private readonly IConvertJsonValueToIndexNormalForm _normalFormConverter;

        public JsonFilter(IJsonValue filter, IConvertJsonValueToIndexNormalForm normalFormConverter = null)
        {
            _filter = filter;
            _normalFormConverter = normalFormConverter;
        }

        #region IJsonFilter Members

        public bool Matches(IJsonValue value)
        {
            return Matches(_filter, _normalFormConverter == null ? value : _normalFormConverter.Convert(value));
        }

        #endregion

        private bool Matches(IJsonValue filter, IJsonValue value)
        {
            if ((filter == null) || (value == null))
            {
                return filter == value;
            }

            var comparer = filter.Visit(new CreateFilterComparerVisitor());
            return value.Visit(comparer);
        }
    }
}