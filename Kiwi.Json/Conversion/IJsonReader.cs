using Kiwi.Json.Conversion.TypeBuilders;

namespace Kiwi.Json.Conversion
{
    public interface IJsonReader
    {
        bool EndOfInput();
        object Parse(ITypeBuilder builder, object instanceState);
    }
}