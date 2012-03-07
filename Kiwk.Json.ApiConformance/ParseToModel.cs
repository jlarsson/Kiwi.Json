using System.Collections.Generic;
using System.ComponentModel;

namespace Kiwk.Json.ApiConformance
{
    public static class X
    {
        private IEnumerable<IConformanceTest> tests = new IConformanceTest[]
                                                          {
                                                              new ParseToModel<int>("123", 123),
                                                              new ParseToModel<int?>("123", 123),
                                                              new ParseToModel<int?>("null", null)
                                                          };
    }



    public class ParseToModel<T>: IConformanceTest
    {
        private readonly string _json;
        private readonly T _expected;

        public ParseToModel(string json, T expected)
        {
            _json = json;
            _expected = expected;
        }

        public bool TestConformance(IJsonApi api)
        {
            var value = api.ParseModel<T>(_json);

            return Equals(value, _expected);
        }
    }
}
