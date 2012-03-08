using System.Collections.Generic;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase
{
    public static class DocumentCollectionExtensions
    {
        public static void EnsureIndex(this IDocumentCollection collection, IndexDefinition definition)
        {
            using (var session = collection.CreateSession())
            {
                session.EnsureIndex(definition);
                session.Commit();
            }
        }

        public static IEnumerable<KeyValuePair<string, IJsonValue>> Find(this IDocumentCollection collection, object filter)
        {
            using (var session = collection.CreateSession())
            {
                return session.Find<IJsonValue>(JSON.ToJson(filter));
            }
        }

        public static T Get<T>(this IDocumentCollection collection, string key)
        {
            using (var session = collection.CreateSession())
            {
                return session.Get<T>(key);
            }
        }

        public static void Put(this IDocumentCollection collection, string key, object document)
        {
            using (var session = collection.CreateSession())
            {
                session.Put(key, JSON.ToJson(document));
                session.Commit();
            }
        }
    }
}