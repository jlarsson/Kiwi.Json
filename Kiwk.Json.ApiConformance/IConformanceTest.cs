namespace Kiwk.Json.ApiConformance
{
    public interface IConformanceTest
    {
        bool TestConformance(IJsonApi api);
    }
}