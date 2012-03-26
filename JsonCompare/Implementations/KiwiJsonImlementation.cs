using Kiwi.Json;

namespace JsonCompare.Implementations
{
    public class KiwiJsonImlementation: IJsonImplementation
    {
        public string Name
        {
            get { return "Kiwi.Json"; }
        }

        public T Parse<T>(string jsonEncoding)
        {
            return JsonConvert.Parse<T>(jsonEncoding);
        }

        public string Write<T>(T value)
        {
            return JsonConvert.Write(value);
        }
    }
}
