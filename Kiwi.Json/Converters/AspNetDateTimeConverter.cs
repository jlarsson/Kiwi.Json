using System;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Converters
{
    public class AspNetDateTimeConverter : AbstractJsonConverter
    {
        private static readonly DateTime BaseJavaScriptDate = new DateTime(1970, 1, 1);

        public override ITypeBuilder CreateTypeBuilder(Type type)
        {
            return TryCreateTypeBuilder<DateTime, string>(type, ParseDate)
                   ?? TryCreateTypeBuilder<DateTime?, string>(type, s => ParseDate(s));
        }

        public override ITypeWriter CreateTypeWriter(Type type)
        {
            return TryCreateWriter<DateTime>(type, 
                dt => new JsonLiteralContent(string.Concat(@"""\/Date(",(dt - BaseJavaScriptDate).TotalMilliseconds,@")\/""")));
        }

        private DateTime ParseDate(string s)
        {
            return BaseJavaScriptDate.Add(
                TimeSpan.FromMilliseconds(long.Parse(s.Substring(6, s.IndexOf(')') - 6))));
        }
    }
}