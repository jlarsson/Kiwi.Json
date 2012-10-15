using System;
using System.Linq;
using Kiwi.Json.Util;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class TypeWriterRegistry : ICustomizableTypeWriterRegistry
    {
        private readonly ThreadSafeList<IJsonConverter> _customConverters = new ThreadSafeList<IJsonConverter>();

        private readonly ITypeWriterFactory[] _factories = new ITypeWriterFactory[]
                                                               {
                                                                   new BuiltinTypeWriterFactory(),
                                                                   new JsonValueWriterFactory(),
                                                                   new SystemObjectWriterFactory(),
                                                                   new DictionaryWriterFactory(),
                                                                   new EnumerableWriterFactory(),
                                                                   new ClassWriterFactory(),
                                                                   new EnumWriterFactory(),
                                                                   new StructWriterFactory()
                                                               };

        private readonly ITypeWriter _nullWriter = new NullWriter();

        private readonly IRegistry<Type, ITypeWriter> _serializers = new ThreadsafeRegistry<Type, ITypeWriter>();

        #region ICustomizableTypeWriterRegistry Members

        public ITypeWriter GetTypeWriterForValue(object value)
        {
            if (value == null)
            {
                return _nullWriter;
            }
            return GetTypeWriterForType(value.GetType());
        }

        public ITypeWriter GetTypeWriterForType(Type type)
        {
            return _serializers.Lookup(type, CreateTypeSerializerForType);
        }

        public void RegisterConverters(IJsonConverter[] converters)
        {
            _customConverters.Add(converters);
        }

        #endregion

        private ITypeWriter CreateTypeSerializerForType(Type type)
        {
            return _customConverters.Select(c => c.CreateTypeWriter(type)).FirstOrDefault(w => w != null)
                   ?? _factories.Select(f => f.CreateTypeWriter(type)).FirstOrDefault(w => w != null)
                   ?? NothingTypeWriter.Instance;
        }

        #region Nested type: NullWriter

        private class NullWriter : ITypeWriter
        {
            #region ITypeWriter Members

            public void Write(IJsonWriter writer, ITypeWriterRegistry registry, object value)
            {
                writer.WriteNull();
            }

            #endregion
        }

        #endregion
    }
}