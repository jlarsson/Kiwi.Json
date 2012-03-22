using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;
using Kiwi.Json.Untyped;

namespace Kiwi.Json
{
    public interface IJsonConvert
    {
        void RegisterCustomConverters(params IJsonConverter[] customConverters);
        ITypeWriterRegistry CreateTypeWriterRegistry(IJsonConverter[] customConverters);
        ITypeBuilderRegistry CreateTypeBuilderRegistry(IJsonConverter[] customConverters);
        IJsonValue ToJson(object obj, params IJsonConverter[] customConverters);
        T ToObject<T>(IJsonValue value, params IJsonConverter[] customConverters);
        T Parse<T>(IJsonParser parser, T initializedInstance, params IJsonConverter[] customConverters);
        void Write(object obj, IJsonWriter writer, params IJsonConverter[] customConverters);
        IJsonValue MergeWith(IJsonValue value, IJsonValue mergeWith);
    }
}