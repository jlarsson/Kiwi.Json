using Kiwi.Json.Conversion.TypeBuilders;

namespace Kiwi.Json.Conversion
{
    public interface IJsonParser
    {
        bool EndOfInput();
        object Parse(ITypeBuilderRegistry registry, ITypeBuilder builder, object instanceState);
    }
}