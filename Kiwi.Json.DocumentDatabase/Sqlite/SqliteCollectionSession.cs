using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.DocumentDatabase.Indexing;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class SqliteCollectionSession : ICollectionSession
    {
        public SqliteCollectionSession(IDbSession dbSession, IDocumentCollection collection)
        {
            DbSession = dbSession;
            Collection = collection;
            FilterStrategy = new FilterStrategy();
        }

        private IDbSession DbSession { get; set; }
        private IDocumentCollection Collection { get; set; }

        public IJsonFilterStrategy FilterStrategy { get; set; }

        #region ICollectionSession Members

        public void Dispose()
        {
            DbSession.Dispose();
        }

        public void Commit()
        {
            DbSession.Commit();
        }

        public void Rollback()
        {
            DbSession.Rollback();
        }

        public void Pulse()
        {
            
        }

        public void EnsureIndex(IndexDefinition definition)
        {
            var existingIndex = DbSession.CreateSqlCommand(
                @"SELECT Definition FROM CollectionIndex CI INNER JOIN DocumentCollection C ON C.CollectionId = C.CollectionId WHERE CI.JsonPath = @jsonPath AND C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("jsonPath", definition.JsonPath)
                .Query(a => JsonConvert.Read<IndexDefinition>(a.String(0)))
                .FirstOrDefault();

            if (existingIndex != null)
            {
                return;
            }

            // Create the index in the database
            EnsureCollectionExistsInDatabase();

            var indexId = DbSession.CreateSqlCommand(
                @"INSERT INTO CollectionIndex (CollectionId, JsonPath, Definition) SELECT CollectionId, @jsonPath, @definition FROM DocumentCollection WHERE CollectionName = @collection; SELECT last_insert_rowid();")
                .Param("collection", Collection.Name)
                .Param("jsonPath", definition.JsonPath)
                .Param("definition", JsonConvert.Write(definition))
                .Query(a => a.Long(0)).First();


            var documents = from document in
                                DbSession.CreateSqlCommand(
                                    "SELECT DocumentId, Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                                .Param("collection", Collection.Name)
                                .Query(a => new {DocumentId = a.Long(0), Document = JsonConvert.Read(a.String(1))})
                            select document;


            var indexValues = from document in documents
                              from indexValue in definition.JsonPath.Evaluate(document.Document)
                              select new {document.DocumentId, IndexValue = indexValue};

            foreach (var indexValue in indexValues)
            {
                DbSession.CreateSqlCommand(
                    "INSERT INTO CollectionIndexValues (CollectionIndexId, DocumentId, Json) VALUES (@indexId, @documentId, @json)")
                    .Param("indexId", indexId)
                    .Param("documentId", indexValue.DocumentId)
                    .Param("json", JsonConvert.Write(indexValue.IndexValue))
                    .Execute();
            }
        }

        public IEnumerable<KeyValuePair<string, T>> Find<T>(IJsonValue filter)
        {
            var alldocs = DbSession
                .CreateSqlCommand(
                    "SELECT D.[Key], D.Json, C.CollectionName FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId")
                .Query(a => new {Key = a.String(0), Json = a.String(1), Collection = a.String(2)})
                .ToArray();

            var indices = DbSession.CreateSqlCommand(
                "SELECT CI.CollectionIndexId, CI.JsonPath FROM CollectionIndex CI INNER JOIN DocumentCollection C ON CI.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Query(a => new
                                {
                                    IndexId = a.Long(0),
                                    JsonPath = JsonConvert.ParseJsonPath(a.String(1)),
                                });

            var restrictions = (
                                   from index in indices
                                   from filterValue in index.JsonPath.Evaluate(filter)
                                   select new {index.IndexId, IndexValue = filterValue}).ToArray();

            var command = default (IDatabaseCommand);
            if (restrictions.Any())
            {
                var sql = "SELECT D.[Key], D.Json FROM Document D "
                          +
                          string.Join(" ",
                                      restrictions.Select(
                                          (r, i) =>
                                          string.Format(
                                              "INNER JOIN CollectionIndexValue CIV{0} ON D.DocumentId = CIV{0}.DocumentId AND CIV{0}.Json = @v{0} AND CIV{0}.CollectionIndexId = @civid{0}",
                                              i)));

                command = DbSession.CreateSqlCommand(sql);
                for (var i = 0; i < restrictions.Length; ++i)
                {
                    command.Param("v" + i, JsonConvert.Write(restrictions[i].IndexValue));
                    command.Param("civid" + i, restrictions[i].IndexId);
                }
            }
            else
            {
                command = DbSession
                    .CreateSqlCommand(
                        "SELECT D.[Key], D.Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                    .Param("collection", Collection.Name);
            }

            var f = FilterStrategy.CreateFilter(filter);
            return
                (from kv in
                     command.Query(a => new KeyValuePair<string, IJsonValue>(a.String(0), JsonConvert.Read(a.String(1))))
                 where f.Matches(kv.Value)
                 select new KeyValuePair<string, T>(kv.Key, kv.Value.ToObject<T>())
                ).ToList();
        }

        public T Get<T>(string key)
        {
            return DbSession.CreateSqlCommand(
                @"SELECT D.Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE D.[Key] = @key AND C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("key", key)
                .Query(a => JsonConvert.Read<T>(a.String(0)))
                .FirstOrDefault();
        }

        public void Put(string key, IJsonValue document)
        {
            EnsureCollectionExistsInDatabase();

            var documentId = default(long);
            var oldDocumentId = DbSession.CreateSqlCommand(
                "SELECT D.DocumentId FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE D.[Key] = @key AND C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("key", key)
                .Query(a => (long?) a.Long(0)).FirstOrDefault();
            if (oldDocumentId.HasValue)
            {
                documentId = oldDocumentId.Value;
                DbSession.CreateSqlCommand(
                    @"UPDATE Document SET Json = @json WHERE DocumentId = @documentId")
                    .Param("documentId", documentId)
                    .Param("json", JsonConvert.Write(document))
                    .Execute();
            }
            else
            {
                documentId = DbSession.CreateSqlCommand(
                    @"INSERT INTO Document ([Key],Json, CollectionId) SELECT @key,@json,CollectionId FROM DocumentCollection WHERE CollectionName = @collection; SELECT last_insert_rowid();")
                    .Param("collection", Collection.Name)
                    .Param("key", key)
                    .Param("json", JsonConvert.Write(document))
                    .Query(a => a.Long(0))
                    .First();
            }

            var indices = DbSession.CreateSqlCommand(
                "SELECT CollectionIndexId, JsonPath FROM CollectionIndex CI INNER JOIN DocumentCollection C ON CI.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Query(a => new
                                {
                                    IndexId = a.Long(0),
                                    JsonPath = JsonConvert.ParseJsonPath(a.String(1))
                                });

            if (oldDocumentId.HasValue)
            {
                DbSession
                    .CreateSqlCommand("DELETE FROM CollectionIndexValue WHERE DocumentId = @documentId")
                    .Param("documentId", documentId)
                    .Execute();
            }
            foreach (var index in indices)
            {
                var indexValues = from pathMatch in index.JsonPath.Evaluate(document)
                                  from filterValue in FilterStrategy.GetFilterValues(pathMatch)
                                  select filterValue;

                foreach (var indexValue in indexValues)
                {
                    DbSession.CreateSqlCommand(
                        "INSERT INTO CollectionIndexValue (CollectionIndexId, DocumentId, Json) VALUES(@indexId, @documentId, @json)")
                        .Param("indexId", index.IndexId)
                        .Param("documentId", documentId)
                        .Param("json", JsonConvert.Write(indexValue))
                        .Execute();
                }
            }
        }

        public void Remove(string key)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private void EnsureCollectionExistsInDatabase()
        {
            DbSession.CreateSqlCommand(
                @"INSERT OR IGNORE INTO DocumentCollection (CollectionName) VALUES (@collection)")
                .Param("collection", Collection.Name)
                .Execute();
        }
    }
}