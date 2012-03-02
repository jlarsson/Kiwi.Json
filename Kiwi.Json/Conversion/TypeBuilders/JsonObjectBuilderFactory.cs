using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonObjectBuilderFactory : ITypeBuilderFactory
    {
        public Func<ITypeBuilder> CreateTypeBuilder(Type type, ITypeBuilderRegistry registry)
        {
            if (typeof(IJsonObject).IsAssignableFrom(type))
            {
                var builder = new JsonObjectBuilder();
                return () => builder;
            }
            return null;
        }
    }
}