using System;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;

namespace Kiwi.Json.Converters
{
    public class UriConverter: AbstractJsonConverter
    {
        public override ITypeBuilder CreateTypeBuilder(Type type)
        {
            return TryCreateTypeBuilder<Uri, string>(type, s => new Uri(s));
        }

        public override ITypeWriter CreateTypeWriter(Type type)
        {
            return TryCreateWriter<Uri>(type, uri => uri.ToString());
        }
    }
}