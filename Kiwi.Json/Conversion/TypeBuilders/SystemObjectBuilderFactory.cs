using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class SystemObjectBuilderFactory: ITypeBuilderFactory
    {
        public Func<ITypeBuilder> CreateTypeBuilder(Type type, ITypeBuilderRegistry registry)
        {
            if (type == typeof(object))
            {
                var builder = new SystemObjectBuilder();
                return () => builder;
            }
            return null;
        }
    }
}