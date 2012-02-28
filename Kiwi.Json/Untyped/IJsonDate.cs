using System;

namespace Kiwi.Json.Untyped
{
    public interface IJsonDate : IJsonValue
    {
        DateTime Value { get; }
    }
}