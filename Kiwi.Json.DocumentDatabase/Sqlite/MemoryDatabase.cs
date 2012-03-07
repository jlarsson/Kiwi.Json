using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class MemoryDatabase : IDatabase
    {
        public MemoryDatabase()
        {
            Connection = new SQLiteConnection(@"Data Source=:memory:");
            Connection.Open();

            EnforceSchema();
        }

        protected SQLiteConnection Connection { get; private set; }

        #region IDatabase Members

        public IEnumerable<IDocumentCollection> Collections
        {
            get
            {
                using (var session = CreateSession())
                {
                    return session.CreateSqlCommand("SELECT DocumentCollectionName FROM DocumentCollections")
                        .Query(r => GetCollection(r.GetString(0)));
                }
            }
        }

        public IDocumentCollection GetCollection(string name)
        {
            return new SqliteDocumentCollection(name, this);
        }

        #endregion

        private void EnforceSchema()
        {
            using (var session = CreateSession())
            {
                var sql = new StreamReader(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(
                        "Kiwi.Json.DocumentDatabase.sqlite-schema.sql")).ReadToEnd();

                session.DatabaseCommandFactory.CreateSqlCommand(sql).Execute();
/*
                session.DatabaseCommandFactory.CreateSqlCommand(@"
CREATE TABLE IF NOT EXISTS DocumentCollections(
    DocumentCollectionId INTEGER PRIMARY KEY AUTOINCREMENT, 
    DocumentCollectionName TEXT COLLATE NOCASE UNIQUE
)")
                 .Execute();
                session.DatabaseCommandFactory.CreateSqlCommand(@"
CREATE TABLE IF NOT EXISTS Documents( 
    DocumentKey TEXT COLLATE NOCASE, 
    DocumentAsJson TEXT, 
    DocumentCollectionId INTEGER REFERENCES DocumentCollections (DocumentCollectionId) ON DELETE CASCADE,
    PRIMARY KEY (DocumentCollectionId, DocumentKey)
)")
                    .Execute();
                session.DatabaseCommandFactory.CreateSqlCommand(@"
CREATE TABLE IF NOT EXISTS CollectionIndices(
    DocumentCollectionId INT REFERENCES DocumentCollections (DocumentCollectionId) ON DELETE CASCADE, 
    JPath TEXT UNIQUE
)")
                    .Execute();
 */
            }
        }

        protected SqliteCollectionSession CreateSession()
        {
            return new SqliteCollectionSession(Connection, null);
        }

        protected internal SqliteCollectionSession CreateCollectionSession(IDocumentCollection collection)
        {
            return new SqliteCollectionSession(Connection, collection);
        }

        public void Dump()
        {
            Console.Out.WriteLine("## dump");
            var collections = CreateSession().CreateSqlCommand(
                "SELECT DocumentCollectionName, DocumentCollectionId FROM DocumentCollections")
                .Query(r => new {Name = r.GetString(0), Id = r.GetInt32(1)});

            foreach (var collection in collections)
            {
                Console.Out.WriteLine("collection {0}: {1}", collection.Id, collection.Name);
            }

            var documents = CreateSession().CreateSqlCommand(
                "SELECT D.DocumentKey, D.DocumentAsJson, C.DocumentCollectionName FROM Documents D INNER JOIN DocumentCollections C ON D.DocumentCollectionId = C.DocumentCollectionId")
                .Query(r => new {Key = r.GetString(0), Json = r.GetString(1), Collection = r.GetString(2)});

            foreach (var document in documents)
            {
                Console.Out.WriteLine("{0} ({1}): {2}", document.Key, document.Collection, document.Json);
            }
        }
    }
}