using System;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;

namespace Kiwi.Json.Conversion
{
    public interface IJsonConverter
    {
        ITypeBuilder CreateTypeBuilder(Type type);
        ITypeWriter CreateTypeWriter(Type type);
    }
}