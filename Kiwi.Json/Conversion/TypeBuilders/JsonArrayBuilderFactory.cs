using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonArrayBuilderFactory : ITypeBuilderFactory
    {
        public Func<ITypeBuilder> CreateTypeBuilder(Type type, ITypeBuilderRegistry registry)
        {
            if (typeof(IJsonArray).IsAssignableFrom(type))
            {
                var builder = new JsonArrayBuilder();
                return () => builder;
            }
            return null;
        }
    }
}