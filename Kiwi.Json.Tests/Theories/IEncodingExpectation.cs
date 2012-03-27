using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Tests.Theories
{
    public interface IEncodingExpectation
    {
        Type ValueType { get; }
        Type JsonType { get; }
        object Value { get; }
        string ExpectedJsonEncoding { get; }

        IEncodingExpectation ShouldParseTo<T>() where T: IJsonValue;
    }
}