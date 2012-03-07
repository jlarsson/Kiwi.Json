using System.Collections.Generic;

namespace Kiwi.Json.DocumentDatabase
{
    public interface IDatabase
    {
        IEnumerable<IDocumentCollection> Collections { get; }
        IDocumentCollection GetCollection(string name);
    }

    public static class DocumentCollectionExtensions
    {
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
                session.Put(key, document);
            }
        }
    }
}