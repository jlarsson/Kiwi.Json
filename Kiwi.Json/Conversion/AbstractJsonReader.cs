using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Util;

namespace Kiwi.Json.Conversion
{
    public abstract class AbstractJsonReader
    {
        private static readonly Regex MatchDate = new Regex(@"\/Date\((\d+)\)\/");

        protected AbstractJsonReader()
        {
            Column = 1;
            Line = 1;
        }

        public int Line { get; private set; }
        public int Column { get; private set; }

        public object Parse(ITypeBuilder builder)
        {
            return Parse(builder, null);
        }

        public object Parse(ITypeBuilder builder, object instanceState)
        {
            SkipWhitespace();

            var c = Peek();
            switch (c)
            {
                case '{':
                    return ParseObject(builder, instanceState);
                case '[':
                    return ParserArray(builder, instanceState);
                case '\"':
                    return ParseString(builder);
                case 't':
                    return ParseTrue(builder);
                case 'f':
                    return ParseFalse(builder);
                case 'n':
                    return ParseNull(builder);
                default:
                    if (char.IsDigit((char) c) || (c == '-'))
                    {
                        return ParseNumber(builder);
                    }
                    break;
            }
            throw CreateException("Expected Json at ({1},{2})", c, Line, Column);
        }

        /*
         * number = [ minus ] int [ frac ] [ exp ]
         * decimal-point = %x2E       ; .
         * digit1-9 = %x31-39         ; 1-9
         * e = %x65 / %x45            ; e E
         * exp = e [ minus / plus ] 1*DIGIT
         * frac = decimal-point 1*DIGIT
         * int = zero / ( digit1-9 *DIGIT )
         * minus = %x2D               ; -
         * plus = %x2B                ; +
         * zero = %x30                ; 0
         */

        private object ParseNumber(ITypeBuilder builder)
        {
            var startLine = Line;
            var startColumn = Column;
            var sb = new StringBuilder();
            if (Peek() == '-')
            {
                sb.Append(Next());
            }
            var hasInteger = false;
            while (char.IsDigit((char) Peek()))
            {
                sb.Append(Next());
                hasInteger = true;
            }
            if (!hasInteger)
            {
                throw CreateExpectedNumberException(startLine, startColumn);
            }

            if (Peek() != '.')
            {
                long intValue;
                if (!long.TryParse(sb.ToString(), out intValue))
                {
                    throw CreateExpectedNumberException(startLine, startColumn);
                }
                return builder.CreateNumber(intValue);
            }

            sb.Append(Next());
            var hasFrac = false;
            while (char.IsDigit((char) Peek()))
            {
                sb.Append(Next());
                hasFrac = true;
            }
            if (!hasFrac)
            {
                throw CreateExpectedNumberException(startLine, startColumn);
            }

            if ("eE".IndexOf((char) Peek()) >= 0)
            {
                sb.Append(Next());

                if ("+-".IndexOf((char) Peek()) >= 0)
                {
                    sb.Append(Next());
                }
                var hasExp = false;
                while (char.IsDigit((char) Peek()))
                {
                    sb.Append(Next());
                    hasExp = true;
                }
                if (!hasExp)
                {
                    throw CreateExpectedNumberException(startLine, startColumn);
                }
            }
            double doubleValue;

            if (!double.TryParse(sb.ToString(),
                                 NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint |
                                 NumberStyles.AllowExponent,
                                 JsonFormats.DoubleFormat, out doubleValue))
            {
                throw CreateExpectedNumberException(startLine, startColumn);
            }
            return builder.CreateNumber(doubleValue);
        }

        protected object ParseFalse(ITypeBuilder builder)
        {
            Match("false");
            return builder.CreateBool(false);
        }

        protected object ParseTrue(ITypeBuilder builder)
        {
            Match("true");
            return builder.CreateBool(true);
        }

        protected object ParseNull(ITypeBuilder builder)
        {
            Match("null");
            return builder.CreateNull();
        }

        protected string ParseString()
        {
            /*
             * string = quotation-mark *char quotation-mark
             * char = unescaped /
             *  escape (
             *      %x22 /          ; "    quotation mark  U+0022
             *      %x5C /          ; \    reverse solidus U+005C
             *      %x2F /          ; /    solidus         U+002F
             *      %x62 /          ; b    backspace       U+0008
             *      %x66 /          ; f    form feed       U+000C
             *      %x6E /          ; n    line feed       U+000A
             *      %x72 /          ; r    carriage return U+000D
             *      %x74 /          ; t    tab             U+0009
             *      %x75 4HEXDIG )  ; uXXXX                U+XXXX
             *      
             * escape = %x5C              ; \
             * quotation-mark = %x22      ; "
             * unescaped = %x20-21 / %x23-5B / %x5D-10FFFF
             */

            var startLine = Line;
            var startColumn = Column;

            Match('"');

            var sb = new StringBuilder();

            var escaped = false;
            while (true)
            {
                var c = Next();
                if (c == char.MinValue)
                {
                    throw CreateBadStringException(startLine, startColumn);
                }
                if (escaped)
                {
                    if (JsonCharacters.IsEscapeStringChar(c))
                    {
                        sb.Append(JsonCharacters.EscapeStringFor(c));
                    }
                    else if (c == 'u')
                    {
                        sb.Append(ParseUnicodeHexEncoding());
                    }
                    else
                    {
                        throw CreateBadStringException(startLine, startColumn);
                    }
                    escaped = false;
                }
                else if (c == '\\')
                {
                    escaped = true;
                }
                else if (JsonCharacters.IsUnescapedStringChar(c))
                {
                    sb.Append(c);
                }
                else if (c == '\"')
                {
                    break;
                }
                else
                {
                    throw CreateBadStringException(startLine, startColumn);
                }
            }
            return sb.ToString();
        }

        private string ParseNumber(out bool isInteger)
        {
            var startLine = Line;
            var startColumn = Column;
            var sb = new StringBuilder();
            if (Peek() == '-')
            {
                sb.Append(Next());
            }
            var hasInteger = false;
            while (char.IsDigit((char) Peek()))
            {
                sb.Append(Next());
                hasInteger = true;
            }
            if (!hasInteger)
            {
                throw CreateExpectedNumberException(startLine, startColumn);
            }

            if (Peek() != '.')
            {
                isInteger = true;
                return sb.ToString();
            }

            sb.Append(Next());
            var hasFrac = false;
            while (char.IsDigit((char) Peek()))
            {
                sb.Append(Next());
                hasFrac = true;
            }
            if (!hasFrac)
            {
                throw CreateExpectedNumberException(startLine, startColumn);
            }

            if ("eE".IndexOf((char) Peek()) >= 0)
            {
                sb.Append(Next());

                if ("+-".IndexOf((char) Peek()) >= 0)
                {
                    sb.Append(Next());
                }
                var hasExp = false;
                while (char.IsDigit((char) Peek()))
                {
                    sb.Append(Next());
                    hasExp = true;
                }
                if (!hasExp)
                {
                    throw CreateExpectedNumberException(startLine, startColumn);
                }
            }

            isInteger = false;
            return sb.ToString();
        }

        protected char ParseUnicodeHexEncoding()
        {
            return (char) (ParseHexCharValue()*0x1000 + ParseHexCharValue()*0x100 + ParseHexCharValue()*0x10 +
                           ParseHexCharValue());
        }

        protected int ParseHexCharValue()
        {
            var c = Next();
            if (('0' <= c) && (c <= '9'))
            {
                return c - '0';
            }
            if (('a' <= c) && (c <= 'f'))
            {
                return c - 'a' + 10;
            }
            if (('A' <= c) && (c <= 'F'))
            {
                return c - 'A' + 10;
            }
            throw CreateException("Bad Hex character at ({0},{1})", Line, Column);
        }

        private string ParseIdent()
        {
            var sb = new StringBuilder();
            var c = Peek();
            while (char.IsLetterOrDigit((char) c) || (c == '_'))
            {
                Next();
                sb.Append((char) c);

                c = Peek();
            }
            return sb.ToString();
        }

        protected object ParseString(ITypeBuilder builder)
        {
            var s = ParseString();

            //if (s.StartsWith("/Date("))
            {
                var m = MatchDate.Match(s);
                if (m.Success)
                {
                    return builder.CreateDateTime(new DateTime((long) ulong.Parse(m.Groups[1].Value)));
                }
            }
            return builder.CreateString(s);
        }

        protected object ParserArray(ITypeBuilder builder, object instanceState)
        {
            Match('[');
            SkipWhitespace();
            var arrayBuilder = builder.CreateArrayBuilder();
            var array = arrayBuilder.CreateNewArray(instanceState);

            while (Peek() != ']')
            {
                arrayBuilder.AddElement(array, Parse(arrayBuilder.GetElementBuilder()));
                SkipWhitespace();

                if (!TryMatch(','))
                {
                    break;
                }
                SkipWhitespace();
            }
            Match(']');

            return arrayBuilder.GetArray(array);
        }

        protected object ParseObject(ITypeBuilder builder, object instanceState)
        {
            Match('{');
            SkipWhitespace();

            var objectBuilder = builder.CreateObjectBuilder();
            var @object = objectBuilder.CreateNewObject(instanceState);

            while (Peek() != '}')
            {
                var memberName = ParseString();
                SkipWhitespace();
                Match(':');

                var memberState = objectBuilder.GetMemberState(memberName, @object);
                objectBuilder.SetMember(memberName, @object, Parse(objectBuilder.GetMemberBuilder(memberName), memberState));

                SkipWhitespace();

                if (!TryMatch(','))
                {
                    break;
                }
                SkipWhitespace();
            }
            Match('}');
            return objectBuilder.GetObject(@object);
        }

        protected bool TryMatch(char c)
        {
            if (Peek() == c)
            {
                Next();
                return true;
            }
            return false;
        }

        protected void Match(char c)
        {
            var curr = Next();
            if (curr != c)
            {
                throw CreateException("Expected '{0}' at ({1},{2})", c, Line, Column);
            }
        }

        protected void Match(string s)
        {
            var startLine = Line;
            var startColumn = Column;
            foreach (var c in s)
            {
                if (Next() != c)
                {
                    throw CreateException("Expected \"{0}\" at ({1},{2})", s, startLine, startColumn);
                }
            }
        }

        protected static Exception CreateException(string format, params object[] args)
        {
            throw new JsonSerializationException(string.Format(format, args));
        }

        protected Exception CreateExpectedNumberException(int startLine, int startColumn)
        {
            return CreateException("Expected number at ({0},{1})", startLine, startColumn);
        }

        protected Exception CreateBadStringException(int startLine, int startColumn)
        {
            throw CreateException("Bad string literal at ({0},{1})", startLine, startColumn);
        }

        protected abstract int Peek();

        protected char Next()
        {
            var c = Read();
            if (c < 0)
            {
                throw CreateException("Unexpected end of input at ({0},{1})", Line, Column);
            }
            if (c == '\n')
            {
                ++Line;
                Column = 1;
            }
            else
            {
                ++Column;
            }
            return (char) c;
        }

        protected virtual void SkipWhitespace()
        {
            while (char.IsWhiteSpace((char) Peek()))
            {
                Next();
            }
        }

        protected abstract int Read();
    }
}