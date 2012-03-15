using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class JsonValueWriterFactory : ITypeWriterFactory
    {
        public Func<ITypeWriter> CreateTypeWriter(Type type)
        {
            if (typeof(IJsonValue).IsAssignableFrom(type))
            {
                return () => new JsonValueWriter();
            }
            return null;
        }
    }
}