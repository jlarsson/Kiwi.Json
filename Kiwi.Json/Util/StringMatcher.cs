using System;
using System.Linq;
using System.Text;
using Kiwi.Json.Conversion;

namespace Kiwi.Json.Util
{
    public class StringMatcher
    {
        private readonly string _source;
        public int Line { get; private set; }
        public int Column { get; private set; }

        public bool EndOfInput { get { return _index >= _source.Length; } }

        private int _index;
        public StringMatcher(string source)
        {
            _source = source;
            Line = 1;
            Column = 1;
        }

        public bool TryMatch(char c)
        {
            if (Peek() == c)
            {
                ++_index;
                return true;
            }
            return false;
        }
        public bool TryMatch(string s)
        {
            var p = _index;
            if (s.All(TryMatch))
            {
                return true;
            }
            _index = p;
            return false;
        }

        public void Match(char c)
        {
            var curr = Next();
            if (curr != c)
            {
                throw CreateExpectedException(c);
            }
        }

        public void Match(string s)
        {
            foreach (var c in s)
            {
                if (Next() != c)
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
                var c = Next();
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
                        sb.Append(ParseUnicodeHexEncoding());
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
                    sb.Append(c);
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
            throw CreateExpectedException("a valid hex character");
        }

        protected char Next()
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

        protected int Read()
        {
            return _index < _source.Length ? _source[_index++] : char.MinValue;
            //return _json[_index++];
        }

        public int Peek()
        {
            return _index < _source.Length ? _source[_index] : char.MinValue;
            //return _json[_index];
        }

        public Exception CreateExpectedException(object expectedWhat)
        {
            throw new JsonSerializationException(string.Format("Expected {0} at ({1},{2}) in {3}", expectedWhat, Line, Column, _source));
        }

        public string MatchIdent()
        {
            var c = Peek();
            var sb = new StringBuilder();
            
            while (char.IsLetterOrDigit((char)c) || (c == '_'))
            {
                Next();
                sb.Append((char)c);

                c = Peek();
            }
            if (sb.Length == 0)
            {
                CreateExpectedException("identifier");
            }
            return sb.ToString();
        }

        public string MatchUntil(char c)
        {
            var sb = new StringBuilder();
            var curr = Peek();
            while ((curr != char.MinValue) && (curr != c))
            {
                sb.Append(Next());
                curr = Peek();
            }
            return sb.ToString();
        }
    }
}