using System.Json;
using System.Runtime.Serialization.Json;

namespace JsonCompare.Implementations
{
    public class SystemJsonImplementation : IJsonImplementation
    {

        public string Name
        {
            get { return "System.Json (BETA)"; }
        }

        public T Parse<T>(string jsonEncoding)
        {
            return JsonValue.Parse(jsonEncoding).ReadAs<T>();
        }

        public string Write<T>(T value)
        {
            return JsonValueExtensions.CreateFrom(value).ToString();
        }
    }
}