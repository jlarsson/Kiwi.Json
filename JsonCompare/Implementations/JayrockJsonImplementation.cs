using Jayrock.Json.Conversion;

namespace JsonCompare.Implementations
{
    public class JayrockJsonImplementation : IJsonImplementation
    {
        public string Name
        {
            get { return "JayRock"; }
        }

        public T Parse<T>(string jsonEncoding)
        {
            return JsonConvert.Import<T>(jsonEncoding);
        }

        public string Write<T>(T value)
        {
            return JsonConvert.ExportToString(value);
        }
    }
}