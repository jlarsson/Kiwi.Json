using System;
using System.Linq;
using Kiwi.Json.Util;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class TypeBuilderRegistry : ITypeBuilderRegistry
    {
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
                                                                    new StructBuilderFactory()
                                                                };

        private readonly IRegistry<Type, ITypeBuilder> _typeBuilders =
            new ThreadsafeRegistry<Type, ITypeBuilder>();

        #region ITypeBuilderRegistry Members

        public ITypeBuilder GetTypeBuilder<T>()
        {
            return GetTypeBuilder(typeof (T));
        }

        public ITypeBuilder GetTypeBuilder(Type type)
        {
            return _typeBuilders.Lookup(type, CreateTypeBuilder);
        }

        #endregion

        private ITypeBuilder CreateTypeBuilder(Type type)
        {
            var creator = _factories.Select(f => f.CreateTypeBuilder(type)).FirstOrDefault(f => f != null);
            return creator == null ? null : creator();
        }
    }
}