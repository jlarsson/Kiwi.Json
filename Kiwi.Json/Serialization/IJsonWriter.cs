using System;
using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization
{
    public interface IJsonWriter
    {
        void WriteObject(IDictionary<string, IJsonValue> obj);
        void WriteArray(IList<IJsonValue> array);
        void WriteString(string value);
        void WriteInteger(int value);
        void WriteDouble(double value);
        void WriteDate(DateTime value);
        void WriteBool(bool value);
        void WriteNull();
    }
}