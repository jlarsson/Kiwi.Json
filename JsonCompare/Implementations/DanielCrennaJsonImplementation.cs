using Json;

namespace JsonCompare.Implementations
{
    public class DanielCrennaJsonImplementation : IJsonImplementation
    {

        public string Name
        {
            get { return "Json by D. Crenna"; }
        }

        public T Parse<T>(string jsonEncoding)
        {
            return JsonParser.Deserialize<T>(jsonEncoding);
        }

        public string Write<T>(T value)
        {
            return JsonParser.Serialize(value);
        }
    }
}