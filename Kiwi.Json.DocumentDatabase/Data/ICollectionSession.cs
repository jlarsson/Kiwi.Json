using System;
using System.Collections.Generic;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public interface ICollectionSession: IDisposable
    {
        void Commit();
        void Rollback();
        void EnsureIndex(IndexDefinition definition);

        IEnumerable<KeyValuePair<string,T>> Find<T>(IJsonValue filter);
        T Get<T>(string key);
        void Put(string key, IJsonValue document);
    }
}