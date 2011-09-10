using System;
using System.Linq;
using System.Text;

namespace Kiwi.Json.Serialization
{
    public static class JsonCharacters
    {
        public static bool IsUnescapedStringChar(char c)
        {
            return (('\x23' <= c) && (c <= '\x5b'))
                   || (('\x20' <= c) && (c <= '\x21'))
                   || (('\x5d' <= c) && (c <= 0x10FFFF));
            //|| (('\x5d' <= c) && (c <= '\x10FFFF'));
        }

        public static bool IsEscapeStringChar(char c)
        {
            return "\x22\x5c\x2f\x62\x66\x6e\x72\x74".Contains(c);
        }

        public static bool IsHexChar(char c)
        {
            return (('0' <= c) && (c <= '9'))
                   || (('a' <= c) && (c <= 'f'))
                   || (('A' <= c) && (c <= 'F'));
        }

        public static char EscapeStringFor(char escape)
        {
            switch (escape)
            {
                case '"':
                    return '"';
                case '\\':
                    return '\\';
                case '/':
                    return '/';
                case 'b':
                    return '\b';
                case 'f':
                    return '\f';
                case 'n':
                    return '\n';
                case 'r':
                    return '\r';
                case 't':
                    return '\t';
            }
            throw new ApplicationException(
                string.Format("Internal error: character '{0}' is not valid in escape sequence.", escape));
        }

        public static string EscapeString(string s)
        {
            if (s.All(IsUnescapedStringChar))
            {
                return s;
            }

            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (IsUnescapedStringChar(c))
                {
                    sb.Append(c);
                }
                else
                {
                    switch (c)
                    {
                        case '"':
                            sb.Append("\\\"");
                            break;
                        case '\\':
                            sb.Append("\\\\");
                            break;
                        case '/':
                            sb.Append("\\/");
                            break;
                        case '\b':
                            sb.Append("\\b");
                            break;
                        case '\f':
                            sb.Append("\\f");
                            break;
                        case '\n':
                            sb.Append("\\n");
                            break;
                        case '\r':
                            sb.Append("\\r");
                            break;
                        case '\t':
                            sb.Append("\\t");
                            break;
                        default:
                            sb.AppendFormat("\\u{0:x4}", c);
                            break;
                    }
                }
            }
            return sb.ToString();
        }
    }
}