using System.IO;

namespace Kiwi.Json.Serialization
{
    public class JsonNoTextIndent : IJsonTextIndent
    {
        #region IJsonTextIndent Members

        public void Increase()
        {
        }

        public void Decrease()
        {
        }

        public void WriteNewLine(TextWriter writer)
        {
        }

        #endregion
    }
}