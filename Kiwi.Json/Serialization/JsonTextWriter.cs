using System;
using System.Collections.Generic;
using System.IO;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization
{
    public class JsonTextWriter : IJsonWriter
    {
        public JsonTextWriter(TextWriter writer) : this(writer, JsonTextIndent.NoIndent)
        {
        }

        public JsonTextWriter(TextWriter writer, string indentString)
            : this(writer, new JsonTextIndent(indentString))
        {
        }

        public JsonTextWriter(TextWriter writer, IJsonTextIndent indent)
        {
            Writer = writer;
            Indent = indent;
        }

        public IJsonTextIndent Indent { get; private set; }

        public TextWriter Writer { get; private set; }

        #region IJsonWriter Members

        public void WriteObject(IDictionary<string, IJsonValue> obj)
        {
            if (obj == null)
            {
                WriteNull();
            }
            else
            {
                WriteBeginTag('{');
                var count = 0;
                foreach (var kv in obj)
                {
                    if (count++ > 0)
                    {
                        Write(',');
                        WriteNewLine();
                    }
                    WriteString(kv.Key);
                    Write(':');
                    Write(kv.Value);
                }
                WriteEndTag('}');
            }
        }

        public void WriteArray(IList<IJsonValue> array)
        {
            if (array == null)
            {
                WriteNull();
            }
            else
            {
                WriteBeginTag('[');
                var count = 0;

                foreach (var value in array)
                {
                    if (count++ > 0)
                    {
                        Write(',');
                        WriteNewLine();
                    }
                    Write(value);
                }
                WriteEndTag(']');
            }
        }

        public void WriteString(string value)
        {
            Write('"');
            Write(JsonCharacters.EscapeString(value));
            Write('"');
        }

        public void WriteInteger(int value)
        {
            Writer.Write(value);
        }

        public void WriteDouble(double value)
        {
            Writer.Write(value.ToString(JsonFormats.DoubleFormat));
        }

        public void WriteDate(DateTime value)
        {
            Writer.Write(@"""\/Date({0})\/""", (ulong) value.ToBinary());
        }

        public void WriteBool(bool value)
        {
            Writer.Write(value ? "true" : "false");
        }

        public void WriteNull()
        {
            Writer.Write("null");
        }

        #endregion

        private void WriteBeginTag(char tag)
        {
            Write(tag);
            Indent.Increase();
            Indent.WriteNewLine(Writer);
        }

        private void Write(char c)
        {
            Writer.Write(c);
        }

        private void Write(string s)
        {
            Writer.Write(s);
        }

        private void WriteNewLine()
        {
            Indent.WriteNewLine(Writer);
        }

        private void WriteEndTag(char tag)
        {
            Indent.Decrease();
            Indent.WriteNewLine(Writer);
            Write(tag);
        }

        private void Write(IJsonValue value)
        {
            if (value == null)
            {
                WriteNull();
            }
            else
            {
                value.Write(this);
            }
        }
    }
}