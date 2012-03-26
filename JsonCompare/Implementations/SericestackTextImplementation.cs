using ServiceStack.Text;

namespace JsonCompare.Implementations
{
    public class SericestackTextImplementation : IJsonImplementation
    {
        #region IJsonImplementation Members

        public string Name
        {
            get { return "Servicestack.Text"; }
        }

        public T Parse<T>(string jsonEncoding)
        {
            return JsonSerializer.DeserializeFromString<T>(jsonEncoding);
        }

        public string Write<T>(T value)
        {
            return JsonSerializer.SerializeToString(value);
        }

        #endregion
    }
}