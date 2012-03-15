using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonValueBuilderFactory : ITypeBuilderFactory
    {
        public Func<ITypeBuilder> CreateTypeBuilder(Type type)
        {
            if (typeof(IJsonValue).IsAssignableFrom(type))
            {
                var builder = new JsonValueBuilder();
                return () => builder;
            }
            return null;
        }
    }
}