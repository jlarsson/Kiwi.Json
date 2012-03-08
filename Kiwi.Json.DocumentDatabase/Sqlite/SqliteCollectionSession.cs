using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using Kiwi.Json.DocumentDatabase.Data;
using Kiwi.Json.DocumentDatabase.Indexing;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class SqliteCollectionSession : ICollectionSession, IDatabaseCommandFactory, IDatabaseCommandExecutor
    {
        private SessionState _state;
        private SQLiteTransaction _transaction;

        public IJsonFilterMatcher Matcher { get; set; }
        public SqliteCollectionSession(SQLiteConnection connection, IDocumentCollection collection)
        {
            Connection = connection;
            Collection = collection;
            Matcher = new FilterMatcher();
        }

        public SQLiteConnection Connection { get; set; }
        public IDocumentCollection Collection { get; set; }

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
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            _state = SessionState.Disposed;
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }
        }

        public void EnsureIndex(IndexDefinition definition)
        {
            var existingIndex = DatabaseCommandFactory.CreateSqlCommand(
                @"SELECT Definition FROM CollectionIndex CI INNER JOIN DocumentCollection C ON C.CollectionId = C.CollectionId WHERE CI.JsonPath = @jsonPath AND C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("jsonPath", definition.JsonPath)
                .Query(r => JSON.Read<IndexDefinition>(r.GetString(0)))
                .FirstOrDefault();

            if (existingIndex != null)
            {
                return;
            }

            // Verify the indexdefinition
            var jsonPath = JSON.ParseJsonPath(definition.JsonPath);

            // Create the index table
            var indexTableName = "CollectionIndex_" + Guid.NewGuid().ToString("n");

            // Create the index in the database
            EnsureCollectionExistsInDatabase();
            DatabaseCommandFactory.CreateSqlCommand(
                string.Format(@"
CREATE TABLE {0} (DocumentId INTEGER REFERENCES Document(DocumentId) ON DELETE CASCADE, Json COLLATION NOCASE TEXT);
CREATE INDEX IX_{0}_DocumentId ON {0} (DocumentId);
CREATE INDEX IX_{0}_Json ON {0} (Json);", indexTableName)).Execute();

            DatabaseCommandFactory.CreateSqlCommand(
                @"INSERT INTO CollectionIndex (CollectionId, JsonPath, Definition, TableName) SELECT CollectionId, @jsonPath, @definition, @tableName FROM DocumentCollection WHERE CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("jsonPath", definition.JsonPath)
                .Param("definition", JSON.Write(definition))
                .Param("tableName", indexTableName)
                .Execute();


            var documents = from document in
                DatabaseCommandFactory.CreateSqlCommand(
                    "SELECT DocumentId, Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                    .Param("collection", Collection.Name)
                    .Query(r => new {DocumentId = r.GetInt64(0), Document = JSON.Read(r.GetString(1))})
            select document;


            var indexValues = from document in documents
            from indexValue in jsonPath.Evaluate(document.Document)
            select new {document.DocumentId, IndexValue = indexValue};

            foreach (var indexValue in indexValues)
            {
                DatabaseCommandFactory.CreateSqlCommand(string.Format("INSERT INTO {0} (DocumentId, Json) VALUES (@documentId, @json)", indexTableName))
                    .Param("documentId", indexValue.DocumentId)
                    .Param("json", indexValue.IndexValue)
                    .Execute();
            }
        }

        public IEnumerable<KeyValuePair<string,T>> Find<T>(IJsonValue filter)
        {
            var indices = DatabaseCommandFactory.CreateSqlCommand(
                "SELECT JsonPath, TableName FROM CollectionIndex CI INNER JOIN DocumentCollection C ON CI.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Query(r => new
                                {
                                    JsonPath = JSON.ParseJsonPath(r.GetString(0)),
                                    IndexTableName = r.GetString(1)
                                });

            var restrictions = (
                                   from index in indices
                                   from filterValue in index.JsonPath.Evaluate(filter)
                                   select new {index.IndexTableName, IndexValue = filterValue}).ToArray();

            var command = default (IDatabaseCommand);
            if (restrictions.Any())
            {
                var sql = "SELECT D.[Key], D.Json FROM Document D "
                          +
                          string.Join(" ",
                                      restrictions.Select(
                                          (r, i) =>
                                          string.Format(
                                              "INNER JOIN [{0}] I{1} ON D.DocumentId = I{1}.DocumentId AND I{1}.Json = @V{1}",
                                              r.IndexTableName, i)));

                command = DatabaseCommandFactory.CreateSqlCommand(sql);
                for (var i = 0; i < restrictions.Length; ++i )
                {
                    command.Param("V" + i, restrictions[i].IndexValue);
                }
            }
            else
            {
                command = DatabaseCommandFactory
                    .CreateSqlCommand(
                        "SELECT D.[Key], D.Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                    .Param("collection", Collection.Name);
            }

            var matcher = new FilterMatcher();

            return (from kv in command.Query(r => new KeyValuePair<string,IJsonValue>(r.GetString(0), JSON.Read(r.GetString(1))))
                    where matcher.IsFilterMatch(filter, kv.Value)
                    select new KeyValuePair<string, T>(kv.Key, kv.Value.ToObject<T>())).ToList();
        }

        public T Get<T>(string key)
        {
            return DatabaseCommandFactory.CreateSqlCommand(
                @"SELECT D.Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE D.[Key] = @key AND C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("key", key)
                .Query(r => r.GetString(0))
                .Select(JSON.Read<T>)
                .FirstOrDefault();
        }

        public void Put(string key, IJsonValue document)
        {
            EnsureCollectionExistsInDatabase();

            var oldDocument = DatabaseCommandFactory.CreateSqlCommand(
                "SELECT D.DocumentId, D.Json FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE D.[Key] = @key AND C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("key", key)
                .Query(r => new
                                {
                                    DocumentId = r.GetInt64(0),
                                    Document = JSON.Read(r.GetString(0))
                                })
                                .FirstOrDefault();


            DatabaseCommandFactory.CreateSqlCommand(
                @"INSERT OR REPLACE INTO Document ([Key],Json, CollectionId) SELECT @key,@json,CollectionId FROM DocumentCollection WHERE CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("key", key)
                .Param("json", JSON.Write(document))
                .Execute();

            var documentId = oldDocument != null
                                 ? oldDocument.DocumentId
                                 : DatabaseCommandFactory.CreateSqlCommand(
                                     "SELECT D.DocumentId FROM Document D INNER JOIN DocumentCollection C ON D.CollectionId = C.CollectionId WHERE D.[Key] = @key AND C.CollectionName = @collection")
                                       .Param("collection", Collection.Name)
                                       .Param("key", key)
                                       .Query(r => r.GetInt64(0)).First();


            var indices = DatabaseCommandFactory.CreateSqlCommand(
                "SELECT JsonPath, TableName FROM CollectionIndex CI INNER JOIN DocumentCollection C ON CI.CollectionId = C.CollectionId WHERE C.CollectionName = @collection")
                .Param("collection", Collection.Name)
                .Query(r => new
                {
                    JsonPath = JSON.ParseJsonPath(r.GetString(0)),
                    IndexTableName = r.GetString(1)
                });

            if (oldDocument != null)
            {
                foreach (var index in indices)
                {
                    DatabaseCommandFactory
                        .CreateSqlCommand(string.Format("DELETE [{0}] WHERE DocumentId = @documentId", index.IndexTableName))
                        .Param("documentId", oldDocument.DocumentId)
                        .Execute();
                }
            }
            foreach (var index in indices)
            {
                var indexValues = from pathMatch in index.JsonPath.Evaluate(document)
                                  from filterValue in Matcher.GetFilterValues(pathMatch)
                                  select filterValue;

                foreach (var indexValue in indexValues)
                {
                    DatabaseCommandFactory.CreateSqlCommand(string.Format("INSERT INTO [{0}] (DocumentId, Json) VALUES(@documentId, @json)", index.IndexTableName))
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
                .Param("collection", Collection.Name)
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
            if (_transaction == null)
            {
                _transaction = Connection.BeginTransaction();
            }

            try
            {
                using (var dbcommand = Connection.CreateCommand())
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

                    dbcommand.Transaction = _transaction;
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