using System;
using System.Linq;
using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Util;

namespace Kiwi.Json
{
    public class CustomTypeBuilderRegistry: ITypeBuilderRegistry
    {
        private readonly ITypeBuilderRegistry _typeBuilderRegistry;
        private readonly IJsonConverter[] _customConverters;
        private readonly IRegistry<Type, ITypeBuilder> _registry = new ThreadsafeRegistry<Type, ITypeBuilder>();
        public CustomTypeBuilderRegistry(ITypeBuilderRegistry typeBuilderRegistry, IJsonConverter[] customConverters)
        {
            _typeBuilderRegistry = typeBuilderRegistry;
            _customConverters = customConverters;
        }


        public ITypeBuilder GetTypeBuilder<T>()
        {
            return GetTypeBuilder(typeof (T));
        }

        public ITypeBuilder GetTypeBuilder(Type type)
        {
            return _registry.Lookup(type,
                                    t =>
                                    _customConverters.Select(c => c.CreateTypeBuilder(t)).FirstOrDefault() ??
                                    _typeBuilderRegistry.GetTypeBuilder(t));
        }
    }
}