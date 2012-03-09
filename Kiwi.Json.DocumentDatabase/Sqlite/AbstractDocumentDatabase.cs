using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public abstract class AbstractDocumentDatabase : IDocumentDatabase
    {
        #region IDatabase Members

        public IEnumerable<IDocumentCollection> Collections
        {
            get
            {
                using (var session = CreateSession())
                {
                    return session.CreateSqlCommand("SELECT CollectionName FROM DocumentCollection")
                        .Query(a => GetCollection(a.String(0)));
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

                session.CreateSqlCommand(sql).Execute();

                session.Commit();
            }
        }

        protected abstract IDbSession CreateSession();

        protected internal SqliteCollectionSession CreateCollectionSession(IDocumentCollection collection)
        {
            return new SqliteCollectionSession(CreateSession(), collection);
        }

        public void Dump()
        {
            Console.Out.WriteLine("## dump");
            using (var session = CreateSession())
            {
                var collections = session.CreateSqlCommand(
                    "SELECT CollectionName, CollectionId FROM DocumentCollection")
                    .Query(a => new {Name = a.String(0), Id = a.Long(1)});

                foreach (var collection in collections)
                {
                    Console.Out.WriteLine("collection {0}: {1}", collection.Id, collection.Name);
                }
            }
            using (var session = CreateSession())
            {
                var documents = session.CreateSqlCommand(
                    "SELECT D.DocumentId, D.[Key], D.Json, C.CollectionName FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId")
                    .Query(a => new {Id = a.Long(0), Key = a.String(1), Json = a.String(2), Collection = a.String(3)});

                foreach (var document in documents)
                {
                    Console.Out.WriteLine("{0} ({1}): {2}", document.Key, document.Collection, document.Json);
                }
            }

            using (var session = CreateSession())
            {
                var indexValues = session.CreateSqlCommand(
                    "SELECT C.CollectionName, D.[Key], CIV.Json, CI.JsonPath FROM CollectionIndexValue CIV INNER JOIN CollectionIndex CI ON CI.CollectionIndexId = CIV.CollectionIndexId INNER JOIN DocumentCollection C ON CI.CollectionId = C.CollectionID INNER JOIN Document D ON D.DocumentId = CIV.DocumentId")
                    .Query(
                        a =>
                        new {Collection = a.String(0), Key = a.String(1), Json = a.String(2), JsonPath = a.String(3)});

                foreach (var iv in indexValues)
                {
                    Console.Out.WriteLine("{0} ({1}) => {2} ({3})", iv.Key, iv.Collection, iv.Json, iv.JsonPath);
                }
            }
        }
    }
}