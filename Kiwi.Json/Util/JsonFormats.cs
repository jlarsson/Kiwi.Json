using System;
using System.Globalization;

namespace Kiwi.Json.Util
{
    public static class JsonFormats
    {
        static JsonFormats()
        {
            DoubleFormat = CultureInfo.GetCultureInfo(1033).NumberFormat;
        }

        public static IFormatProvider DoubleFormat { get; private set; }
    }
}