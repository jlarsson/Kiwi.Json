using System.IO;

namespace Kiwi.Json.Serialization
{
    public class JsonTextIndent : IJsonTextIndent
    {
        static JsonTextIndent()
        {
            NoIndent = new JsonNoTextIndent();
        }

        public JsonTextIndent() : this("\t")
        {
        }

        public JsonTextIndent(string indentString)
        {
            IndentString = indentString;
        }

        public string IndentString { get; set; }
        public static IJsonTextIndent NoIndent { get; private set; }

        protected int Indent { get; set; }

        #region IJsonTextIndent Members

        public void Increase()
        {
            ++Indent;
        }

        public void Decrease()
        {
            --Indent;
        }

        public void WriteNewLine(TextWriter writer)
        {
            writer.WriteLine();
            for (var i = 0; i < Indent; ++i)
            {
                writer.Write(IndentString);
            }
        }

        #endregion
    }
}