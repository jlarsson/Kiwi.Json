namespace JsonCompare
{
    public interface IJsonImplementation
    {
        string Name { get; }
        T Parse<T>(string jsonEncoding);
        string Write<T>(T value);
    }
}