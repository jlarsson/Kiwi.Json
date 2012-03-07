namespace Kiwk.Json.ApiConformance
{
    public interface IJsonApi
    {
        object ParseMapping(string json);
        T ParseModel<T>(string json);

        string Write<T>(T model);
        string WriteMapping(object mapping);


        T ConvertMappingToModel<T>(object mapping);
        object ConvertModelToMapping<T>(T model);
    }
}