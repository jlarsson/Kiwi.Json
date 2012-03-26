using Newtonsoft.Json;

namespace JsonCompare.Implementations
{
    public class NewtonsoftJsonImplementations: IJsonImplementation
    {
        public string Name
        {
            get { return "Newtonsoft.Json"; }
        }

        public T Parse<T>(string jsonEncoding)
        {
            return JsonConvert.DeserializeObject<T>(jsonEncoding);
        }

        public string Write<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}