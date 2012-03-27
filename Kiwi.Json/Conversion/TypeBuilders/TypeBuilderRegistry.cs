using System;
using System.Linq;
using Kiwi.Json.Util;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class TypeBuilderRegistry : ICustomizableTypeBuilderRegistry
    {
        private readonly ThreadSafeList<IJsonConverter> _customConverters = new ThreadSafeList<IJsonConverter>();

        private readonly ITypeBuilderFactory[] _factories = new ITypeBuilderFactory[]
                                                                {
                                                                    new BuiltinTypeBuilderFactory(),
                                                                    new JsonObjectBuilderFactory(),
                                                                    new JsonArrayBuilderFactory(),
                                                                    new JsonValueBuilderFactory(),
                                                                    new SystemObjectBuilderFactory(),
                                                                    new ArrayBuilderFactory(),
                                                                    new DictionaryBuilderFactory(),
                                                                    new CollectionBuilderFactory(),
                                                                    new UntypedListBuilderFactory(),
                                                                    new ClassBuilderFactory(),
                                                                    new EnumBuilderFactory(),
                                                                    new StructBuilderFactory(),
                                                                    new SystemDataNamespaceTypeFactory()
                                                                };

        private readonly IRegistry<Type, ITypeBuilder> _typeBuilders =
            new ThreadsafeRegistry<Type, ITypeBuilder>();

        #region ICustomizableTypeBuilderRegistry Members

        public ITypeBuilder GetTypeBuilder(Type type)
        {
            return _typeBuilders.Lookup(type, CreateTypeBuilder);
        }

        public void RegisterConverters(IJsonConverter[] converters)
        {
            _customConverters.Add(converters);
        }

        #endregion

        private ITypeBuilder CreateTypeBuilder(Type type)
        {
            return _customConverters.Select(c => c.CreateTypeBuilder(type)).FirstOrDefault(w => w != null)
                   ?? _factories.Select(f => f.CreateTypeBuilder(type)).FirstOrDefault(f => f != null);
        }
    }
}