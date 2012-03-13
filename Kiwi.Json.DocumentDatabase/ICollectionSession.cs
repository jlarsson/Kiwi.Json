using System;
using System.Collections.Generic;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase
{
    public interface ICollectionSession: IDisposable
    {
        void Commit();
        void Rollback();
        void Pulse();
        void EnsureIndex(IndexDefinition definition);

        IEnumerable<KeyValuePair<string,T>> Find<T>(IJsonValue filter);
        T Get<T>(string key);
        void Put(string key, IJsonValue document);
        void Remove(string key);
        
    }
}