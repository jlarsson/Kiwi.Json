using System;

namespace Kiwi.Json
{
    public class JsonException : ApplicationException
    {
        public JsonException(string message) : base(message)
        {
        }
    }
}