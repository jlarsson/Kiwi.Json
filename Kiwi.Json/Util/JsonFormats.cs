using System;
using System.Globalization;

namespace Kiwi.Json.Util
{
    public static class JsonFormats
    {
        public static string[] Iso8601Formats { get; set; }

        static JsonFormats()
        {
            DoubleFormat = CultureInfo.GetCultureInfo(1033).NumberFormat;
            Iso8601Formats = new[] {"s", "u"};
        }

        public static IFormatProvider DoubleFormat { get; private set; }

        public static DateTime? TryParseDateTime(string s)
        {
            DateTime dt;
            return DateTime.TryParseExact(s, Iso8601Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) ? (DateTime?)dt : null;
        }
    }
}