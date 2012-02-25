using System;
using System.Text;

namespace Kiwi.Json.Serialization
{
    public class JsonStringWriter : IJsonWriter
    {
        public JsonStringWriter() : this(new StringBuilder())
        {
        }

        public JsonStringWriter(StringBuilder stringBuilder)
        {
            StringBuilder = stringBuilder;
        }

        public StringBuilder StringBuilder { get; private set; }

        #region IJsonWriter Members

        public void WriteString(string value)
        {
            StringBuilder.Append('"');
            StringBuilder.Append(JsonCharacters.EscapeString(value));
            StringBuilder.Append('"');
        }

        public void WriteInteger(long value)
        {
            StringBuilder.Append(value);
        }

        public void WriteDouble(double value)
        {
            StringBuilder.Append(value.ToString(JsonFormats.DoubleFormat));
        }

        public void WriteDate(DateTime value)
        {
            StringBuilder.Append(@"""\/Date(");
            StringBuilder.Append((ulong) value.ToBinary());
            StringBuilder.Append(@")\/""");
        }

        public void WriteBool(bool value)
        {
            StringBuilder.Append(value ? "true" : "false");
        }

        public void WriteNull()
        {
            StringBuilder.Append("null");
        }

        public void WriteArrayStart()
        {
            StringBuilder.Append('[');
        }

        public void WriteArrayElementDelimiter()
        {
            StringBuilder.Append(',');
        }

        public void WriteArrayEnd(int elementCount)
        {
            StringBuilder.Append(']');
        }

        public void WriteObjectStart()
        {
            StringBuilder.Append('{');
        }

        public void WriteMember(string memberName)
        {
            WriteString(memberName);
            StringBuilder.Append(':');
        }

        public void WriteObjectMemberDelimiter()
        {
            StringBuilder.Append(',');
        }

        public void WriteObjectEnd(int memberCount)
        {
            StringBuilder.Append('}');
        }

        #endregion

        public override string ToString()
        {
            return StringBuilder.ToString();
        }
    }
}