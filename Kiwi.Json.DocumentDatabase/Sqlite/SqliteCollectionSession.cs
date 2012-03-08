using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.DocumentDatabase.Indexing;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class SqliteCollectionSession : ICollectionSession, IDatabaseCommandFactory, IDatabaseCommandExecutor
    {
        private readonly ITxFactory _txFactory;
        private readonly IDocumentCollection _collection;
        private ITx _tx;
        private SessionState _state;

        public IJsonFilterStrategy FilterStrategy { get; set; }
        public SqliteCollectionSession(ITxFactory txFactory, IDocumentCollection collection)
        {
            _txFactory = txFactory;
            _collection = collection;
            FilterStrategy = new FilterStrategy();
        }

        public IDatabaseCommandFactory DatabaseCommandFactory
        {
            get { return this; }
        }

        #region ICollectionSession Members

        public void Dispose()
        {
            Rollback();
        }

        public void Commit()
        {
            if (_state == SessionState.Disposed)
            {
                return;
            }
            if (_state != SessionState.Running)
            {
                throw new InvalidSessionStateException();
            }
            _state = SessionState.Disposed;
            if (_tx != null)
            {
                _tx.Commit();
                _tx.Dispose();
                _tx = null;
            }
        }

        public void Rollback()
        {
            _state = SessionState.Disposed;
            if (_tx != null)
            {
                _tx.Rollback();
                _tx.Dispose();
                _tx = null;
            }
        }

        public void EnsureIndex(IndexDefinition definition)
        {
            var existingIndex = DatabaseCommandFactory.CreateSqlCommand(
                @"SELECT Definition FROM CollectionIndex CI INNER JOIN DocumentCollection C ON C.CollectionId = C.CollectionId WHERE CI.JsonPath = @jsonPath AND C.CollectionName = @collection")
                .Param("collection", _collection.Name)
                .Param("jsonPath", definition.JsonPath)
                .Query(r => JSON.Read<IndexDefinition>(r.GetString(0)))
                .FirstOrDefault();

            if (existingIndex != null)
            {
                return;
            }

            // Verify the indexdefinition
            var jsonPath = JSON.ParseJsonPath(definition.JsonPath);

            // Create the index in the database
            EnsureCollectionExistsInDatabase();

            var indexId = DatabaseCommandFactory.CreateSqlCommand(
                    @"INSERT INTO CollectionIndex (CollectionId, JsonPath, Definition) SELECT CollectionId, @jsonPath, @definition FROM DocumentCollection WHERE CollectionName = @collection; SELECT last_insert_rowid();")
                    .Param("collection", _collection.Name)
                    .Param("jsonPath", definition.JsonPath)
                    .Param("definition", JSON.Write(definition))
                    .Query(r => r.GetInt64(0)).First();


            var documents = from document in
                DatabaseCommandFactory.CreateSqlCommand(
                    "SELECT DocumentId, Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                    .Param("collection", _collection.Name)
                    .Query(r => new {DocumentId = r.GetInt64(0), Document = JSON.Read(r.GetString(1))})
            select document;


            var indexValues = from document in documents
            from indexValue in jsonPath.Evaluate(document.Document)
            select new {document.DocumentId, IndexValue = indexValue};

            foreach (var indexValue in indexValues)
            {
                DatabaseCommandFactory.CreateSqlCommand("INSERT INTO CollectionIndexValues (CollectionIndexId, DocumentId, Json) VALUES (@indexId, @documentId, @json)")
                    .Param("indexId", indexId)
                    .Param("documentId", indexValue.DocumentId)
                    .Param("json", JSON.Write(indexValue.IndexValue))
                    .Execute();
            }
        }

        public IEnumerable<KeyValuePair<string,T>> Find<T>(IJsonValue filter)
        {
            var indices = DatabaseCommandFactory.CreateSqlCommand(
                "SELECT CI.CollectionIndexId, CI.JsonPath FROM CollectionIndex CI INNER JOIN DocumentCollection C ON CI.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                .Param("collection", _collection.Name)
                .Query(r => new
                                {
                                    IndexId = r.GetInt64(0),
                                    JsonPath = JSON.ParseJsonPath(r.GetString(1)),
                                    
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

                command = DatabaseCommandFactory.CreateSqlCommand(sql);
                for (var i = 0; i < restrictions.Length; ++i )
                {
                    command.Param("v" + i, JSON.Write(restrictions[i].IndexValue));
                    command.Param("civid" + i, restrictions[i].IndexId);
                }
            }
            else
            {
                command = DatabaseCommandFactory
                    .CreateSqlCommand(
                        "SELECT D.[Key], D.Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                    .Param("collection", _collection.Name);
            }

            var f = FilterStrategy.CreateFilter(filter);
            return (from kv in command.Query(r => new KeyValuePair<string,IJsonValue>(r.GetString(0), JSON.Read(r.GetString(1))))
                    where f.Matches(kv.Value)
                    select new KeyValuePair<string, T>(kv.Key, kv.Value.ToObject<T>())).ToList();
        }

        public T Get<T>(string key)
        {
            return DatabaseCommandFactory.CreateSqlCommand(
                @"SELECT D.Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE D.[Key] = @key AND C.CollectionName = @collection")
                .Param("collection", _collection.Name)
                .Param("key", key)
                .Query(r => r.GetString(0))
                .Select(JSON.Read<T>)
                .FirstOrDefault();
        }

        public void Put(string key, IJsonValue document)
        {
            EnsureCollectionExistsInDatabase();

            var documentId = default(long);
            var oldDocumentId = DatabaseCommandFactory.CreateSqlCommand(
                "SELECT D.DocumentId FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE D.[Key] = @key AND C.CollectionName = @collection")
                .Param("collection", _collection.Name)
                .Param("key", key)
                .Query(r => (long?) r.GetInt64(0)).FirstOrDefault();
            if (oldDocumentId.HasValue)
            {
                documentId = oldDocumentId.Value;
                DatabaseCommandFactory.CreateSqlCommand(
                    @"UPDATE Document SET Json = @json WHERE DocumentId = @documentId")
                    .Param("documentId", documentId)
                    .Param("json", JSON.Write(document))
                    .Execute();
                
            }
            else
            {
                documentId = DatabaseCommandFactory.CreateSqlCommand(
                    @"INSERT INTO Document ([Key],Json, CollectionId) SELECT @key,@json,CollectionId FROM DocumentCollection WHERE CollectionName = @collection; SELECT last_insert_rowid();")
                    .Param("collection", _collection.Name)
                    .Param("key", key)
                    .Param("json", JSON.Write(document))
                    .Query(r => r.GetInt64(0))
                    .First();
            }

            var indices = DatabaseCommandFactory.CreateSqlCommand(
                "SELECT CollectionIndexId, JsonPath FROM CollectionIndex CI INNER JOIN DocumentCollection C ON CI.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                .Param("collection", _collection.Name)
                .Query(r => new
                {
                    IndexId = r.GetInt64(0),
                    JsonPath = JSON.ParseJsonPath(r.GetString(1))
                });

            if (oldDocumentId.HasValue)
            {
                DatabaseCommandFactory
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
                    DatabaseCommandFactory.CreateSqlCommand(
                        "INSERT INTO CollectionIndexValue (CollectionIndexId, DocumentId, Json) VALUES(@indexId, @documentId, @json)")
                        .Param("indexId", index.IndexId)
                        .Param("documentId", documentId)
                        .Param("json", JSON.Write(indexValue))
                        .Execute();
                }
            }
        }

        private void EnsureCollectionExistsInDatabase()
        {
            DatabaseCommandFactory.CreateSqlCommand(
                @"INSERT OR IGNORE INTO DocumentCollection (CollectionName) VALUES (@collection)")
                .Param("collection", _collection.Name)
                .Execute();
        }

        #endregion

        #region IDatabaseCommandExecutor Members

        public void Execute(IDatabaseCommand command)
        {
            ExecuteCommand(command, c => c.ExecuteNonQuery());
        }

        public IEnumerable<T> Query<T>(IDatabaseCommand command, Func<IDataReader, T> map)
        {
            return ExecuteCommand(command, c =>
                                               {
                                                   using (var reader = c.ExecuteReader())
                                                   {
                                                       return Enumerable.Range(0, Int32.MaxValue).TakeWhile(
                                                           i => reader.Read()).Select(i => map(reader)).ToList();
                                                   }
                                               });
        }

        #endregion

        #region IDatabaseCommandFactory Members

        public IDatabaseCommand CreateSqlCommand(string sql)
        {
            return new DbCommandDatabaseCommand(sql, this);
        }

        #endregion

        protected virtual T ExecuteCommand<T>(IDatabaseCommand command, Func<DbCommand, T> executeCommand)
        {
            if (_state != SessionState.Running)
            {
                throw new InvalidSessionStateException();
            }
            if (_tx == null)
            {
                _tx = _txFactory.CreateTransaction();
            }

            try
            {
                using (var dbcommand = _tx.CreateCommand())
                {
                    dbcommand.CommandType = CommandType.Text;
                    dbcommand.CommandText = command.CommandText;
                    foreach (var param in command.Parameters)
                    {
                        var dbparam = dbcommand.CreateParameter();
                        dbparam.ParameterName = param.Key;
                        dbparam.Value = param.Value;
                        dbcommand.Parameters.Add(dbparam);
                    }
                    return executeCommand(dbcommand);
                }
            }
            catch (Exception e)
            {
                _state = SessionState.Failed;
                throw;
            }
        }

        #region Nested type: SessionState

        private enum SessionState
        {
            Running,
            Disposed,
            Failed
        }

        #endregion
    }
}