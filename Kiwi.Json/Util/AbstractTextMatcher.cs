using System;
using System.Text;

namespace Kiwi.Json.Util
{
    public abstract class AbstractTextMatcher
    {
        public int Line { get; private set; }
        public int Column { get; private set; }

        protected AbstractTextMatcher()
        {
            Line = 1;
            Column = 1;
        }

        public abstract bool EndOfInput { get; }

        public bool TryMatch(char c)
        {
            if (PeekNextChar() == c)
            {
                Read();
                return true;
            }
            return false;
        }

        public void Match(char c)
        {
            var curr = MatchAnyChar();
            if (curr != c)
            {
                throw CreateExpectedException(c);
            }
        }

        public void Match(string s)
        {
            foreach (var c in s)
            {
                if (MatchAnyChar() != c)
                {
                    throw CreateExpectedException(s);
                }
            }
        }

        public string MatchString()
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
                var c = MatchAnyChar();
                if (c == char.MinValue)
                {
                    throw CreateExpectedException("more string characters");
                }
                if (escaped)
                {
                    if (JsonCharacters.IsEscapeStringChar(c))
                    {
                        sb.Append(JsonCharacters.EscapeStringFor(c));
                    }
                    else if (c == 'u')
                    {
                        sb.Append((char) ParseUnicodeHexEncoding());
                    }
                    else
                    {
                        throw CreateExpectedException("a valid character escape");
                    }
                    escaped = false;
                }
                else if (c == '\\')
                {
                    escaped = true;
                }
                else if (JsonCharacters.IsUnescapedStringChar(c))
                {
                    sb.Append((char) c);
                }
                else if (c == '\"')
                {
                    break;
                }
                else
                {
                    throw CreateExpectedException("a valid string character");
                }
            }
            return sb.ToString();
        }

        protected char ParseUnicodeHexEncoding()
        {
            return (char)(ParseHexCharValue() * 0x1000 + ParseHexCharValue() * 0x100 + ParseHexCharValue() * 0x10 +
                          ParseHexCharValue());
        }

        protected int ParseHexCharValue()
        {
            var c = MatchAnyChar();
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
            throw CreateExpectedException("a valid hex character");
        }

        public char MatchAnyChar()
        {
            var c = Read();
            if (c < 0)
            {
                throw CreateExpectedException("some more text");
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
            return (char)c;
        }

        protected abstract int Read();

        public abstract int PeekNextChar();

        public abstract Exception CreateExpectedException(object expectedWhat);

        public string MatchIdent(string expectedWhat)
        {
            var c = PeekNextChar();
            var sb = new StringBuilder();
            
            while (char.IsLetterOrDigit((char)c) || (c == '_'))
            {
                MatchAnyChar();
                sb.Append((char)c);

                c = PeekNextChar();
            }
            if (sb.Length == 0)
            {
                CreateExpectedException(expectedWhat);
            }
            return sb.ToString();
        }

        public string MatchUntil(char c)
        {
            var sb = new StringBuilder();
            var curr = PeekNextChar();
            while ((curr != char.MinValue) && (curr != c))
            {
                sb.Append(MatchAnyChar());
                curr = PeekNextChar();
            }
            return sb.ToString();
        }
    }
}