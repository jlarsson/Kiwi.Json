using System;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface ICollectionSession: IDisposable
    {
        T Get<T>(string key);
        void Put(string key, object document);
    }
}