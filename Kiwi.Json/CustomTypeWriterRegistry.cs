using System;
using System.Linq;
using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.TypeWriters;
using Kiwi.Json.Util;

namespace Kiwi.Json
{
    public class CustomTypeWriterRegistry : ITypeWriterRegistry
    {
        private readonly ITypeWriterRegistry _typeWriterRegistry;
        private readonly IJsonConverter[] _customConverters;
        private readonly IRegistry<Type, ITypeWriter> _registry = new ThreadsafeRegistry<Type, ITypeWriter>();

        public CustomTypeWriterRegistry(ITypeWriterRegistry typeWriterRegistry, IJsonConverter[] customConverters)
        {
            _typeWriterRegistry = typeWriterRegistry;
            _customConverters = customConverters;
        }

        public ITypeWriter GetTypeWriterForValue(object value)
        {
            if (value == null)
            {
                return _typeWriterRegistry.GetTypeWriterForValue(null);
            }
            return GetTypeWriterForType(value.GetType());
        }

        public ITypeWriter GetTypeWriterForType(Type type)
        {
            return _registry.Lookup(type, t => _customConverters.Select(c => c.CreateTypeWriter(t)).
                                                   FirstOrDefault
                                                   () ?? _typeWriterRegistry.GetTypeWriterForType(t));
        }
    }
}