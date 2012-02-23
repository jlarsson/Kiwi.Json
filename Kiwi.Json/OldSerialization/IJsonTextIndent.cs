using System.IO;

namespace Kiwi.Json.Serialization
{
    public interface IJsonTextIndent
    {
        void Increase();
        void Decrease();
        void WriteNewLine(TextWriter writer);
    }
}