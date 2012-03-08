using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public abstract class AbstractDatabase : IDatabase
    {
        protected abstract ITxFactory TxFactory { get; }

        #region IDatabase Members

        public IEnumerable<IDocumentCollection> Collections
        {
            get
            {
                using (var session = CreateSession())
                {
                    return session.CreateSqlCommand("SELECT CollectionName FROM DocumentCollection")
                        .Query(r => GetCollection(r.GetString(0)));
                }
            }
        }

        public IDocumentCollection GetCollection(string name)
        {
            return new SqliteDocumentCollection(name, this);
        }

        #endregion

        protected void EnforceSchema()
        {
            using (var session = CreateSession())
            {
                var sql = new StreamReader(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(
                        "Kiwi.Json.DocumentDatabase.sqlite-schema.sql")).ReadToEnd();

                session.DatabaseCommandFactory.CreateSqlCommand(sql).Execute();

                session.Commit();
            }
        }

        protected SqliteCollectionSession CreateSession()
        {
            return new SqliteCollectionSession(TxFactory, null);
        }

        protected internal SqliteCollectionSession CreateCollectionSession(IDocumentCollection collection)
        {
            return new SqliteCollectionSession(TxFactory, collection);
        }

        public void Dump()
        {
            Console.Out.WriteLine("## dump");
            var collections = CreateSession().CreateSqlCommand(
                "SELECT CollectionName, CollectionId FROM DocumentCollection")
                .Query(r => new {Name = r.GetString(0), Id = r.GetInt32(1)});

            foreach (var collection in collections)
            {
                Console.Out.WriteLine("collection {0}: {1}", collection.Id, collection.Name);
            }

            var documents = CreateSession().CreateSqlCommand(
                "SELECT D.DocumentId, D.[Key], D.Json, C.CollectionName FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId")
                .Query(r => new {Id = r.GetInt64(0), Key = r.GetString(1), Json = r.GetString(2), Collection = r.GetString(3)});

            foreach (var document in documents)
            {
                Console.Out.WriteLine("{0} ({1}): {2}", document.Key, document.Collection, document.Json);
            }

            var indexValues = CreateSession().CreateSqlCommand(
                "SELECT C.CollectionName, D.[Key], CIV.Json, CI.JsonPath FROM CollectionIndexValue CIV INNER JOIN CollectionIndex CI ON CI.CollectionIndexId = CIV.CollectionIndexId INNER JOIN DocumentCollection C ON CI.CollectionId = C.CollectionID INNER JOIN Document D ON D.DocumentId = CIV.DocumentId")
                .Query(r => new { Collection = r.GetString(0), Key= r.GetString(1), Json = r.GetString(2), JsonPath = r.GetString(3) });

            foreach (var iv in indexValues)
            {
                Console.Out.WriteLine("{0} ({1}) => {2} ({3})", iv.Key, iv.Collection, iv.Json, iv.JsonPath);
            }

        }
    }
}