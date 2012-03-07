using Kiwi.Json;
using Kiwi.Json.Untyped;

namespace Kiwk.Json.ApiConformance
{
    public class KiwiJsonApi : IJsonApi
    {
        public object ParseMapping(string json)
        {
            return JSON.Parse(json);
        }

        public T ParseModel<T>(string json)
        {
            return JSON.Parse<T>(json);
        }

        public string Write<T>(T model)
        {
            return JSON.Write(model);
        }

        public string WriteMapping(object mapping)
        {
            return JSON.Write(mapping);
        }

        public T ConvertMappingToModel<T>(object mapping)
        {
            return ((IJsonValue) mapping).Map<T>();
        }

        public object ConvertModelToMapping<T>(T model)
        {
            return model.ToJson();
        }
    }
}