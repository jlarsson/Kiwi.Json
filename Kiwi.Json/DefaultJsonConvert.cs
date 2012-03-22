using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;
using Kiwi.Json.Untyped;

namespace Kiwi.Json
{
    public class DefaultJsonConvert : IJsonConvert
    {
        public DefaultJsonConvert()
        {
            TypeWriterRegistry = new TypeWriterRegistry();
            TypeBuilderRegistry = new TypeBuilderRegistry();
        }

        public ICustomizableTypeWriterRegistry TypeWriterRegistry { get; set; }
        public ICustomizableTypeBuilderRegistry TypeBuilderRegistry { get; set; }

        #region IJsonConvert Members

        public void RegisterCustomConverters(params IJsonConverter[] customConverters)
        {
            TypeWriterRegistry.RegisterConverters(customConverters);
            TypeBuilderRegistry.RegisterConverters(customConverters);
        }

        public ITypeWriterRegistry CreateTypeWriterRegistry(IJsonConverter[] customConverters)
        {
            return customConverters.Length == 0
                       ? (ITypeWriterRegistry) TypeWriterRegistry
                       : new CustomTypeWriterRegistry(TypeWriterRegistry, customConverters);
        }

        public ITypeBuilderRegistry CreateTypeBuilderRegistry(IJsonConverter[] customConverters)
        {
            return customConverters.Length == 0
                       ? (ITypeBuilderRegistry) TypeBuilderRegistry
                       : new CustomTypeBuilderRegistry(TypeBuilderRegistry, customConverters);
        }

        public IJsonValue ToJson(object obj, params IJsonConverter[] customConverters)
        {
            var registry = CreateTypeWriterRegistry(customConverters);
            var writer = new ConstructJsonValue();
            registry.Write(writer, obj);
            return writer.GetValue();
        }

        public T ToObject<T>(IJsonValue value, params IJsonConverter[] customConverters)
        {
            var registry = CreateTypeBuilderRegistry(customConverters);
            return (T) value.Visit(new ConvertJsonToCustom(registry, registry.GetTypeBuilder<T>()));
        }

        public T Parse<T>(IJsonParser parser, T initializedInstance, params IJsonConverter[] customConverters)
        {
            return CreateTypeBuilderRegistry(customConverters).Read<T>(parser, initializedInstance);
        }

        public void Write(object obj, IJsonWriter writer, params IJsonConverter[] customConverters)
        {
            CreateTypeWriterRegistry(customConverters).Write(writer, obj);
        }

        public IJsonValue MergeWith(IJsonValue value, IJsonValue mergeWith)
        {
            var registry = TypeBuilderRegistry;
            mergeWith.Visit(new ConvertJsonToCustom(registry, registry.GetTypeBuilder(value.GetType()))
                                {InstanceState = value});
            return value;
        }

        #endregion
    }
}