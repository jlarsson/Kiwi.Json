using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using Kiwi.Json.DocumentDatabase.Data;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class SqliteCollectionSession : ICollectionSession, IDatabaseCommandFactory, IDatabaseCommandExecutor
    {
        private Exception _lastException;
        private SQLiteTransaction _transaction;

        public SqliteCollectionSession(SQLiteConnection connection, IDocumentCollection collection)
        {
            Connection = connection;
            Collection = collection;
        }

        public SQLiteConnection Connection { get; set; }
        public IDocumentCollection Collection { get; set; }

        public IDatabaseCommandFactory DatabaseCommandFactory
        {
            get { return this; }
        }

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

        public void Dispose()
        {
            if (_transaction != null)
            {
                if (_lastException == null)
                {
                    _transaction.Commit();
                }
                else
                {
                    _transaction.Rollback();
                }
            }
        }

        protected virtual T ExecuteCommand<T>(IDatabaseCommand command, Func<DbCommand, T> executeCommand)
        {
            if (_lastException != null)
            {
                throw _lastException;
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
                _lastException = e;
                throw;
            }
        }

        public T Get<T>(string key)
        {
            return DatabaseCommandFactory.CreateSqlCommand(
                    @"SELECT D.DocumentAsJson FROM Documents D INNER JOIN DocumentCollections C ON D.DocumentCollectionId = C.DocumentCollectionId WHERE D.DocumentKey = @key AND C.DocumentCollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("key", key)
                .Query(r => r.GetString(0))
                .Select(JSON.Read<T>)
                .FirstOrDefault();
        }

        public void Put(string key, object document)
        {
            DatabaseCommandFactory.CreateSqlCommand(@"INSERT OR IGNORE INTO DocumentCollections (DocumentCollectionName) VALUES (@collection)")
                .Param("collection", Collection.Name)
                .Execute();

            DatabaseCommandFactory.CreateSqlCommand(
                @"INSERT OR REPLACE INTO Documents (DocumentKey,DocumentAsJson, DocumentCollectionId) SELECT @key,@json,DocumentCollectionId FROM DocumentCollections WHERE DocumentCollectionName = @collection")
                .Param("collection", Collection.Name)
                .Param("key", key)
                .Param("json", JSON.Write(document))
                .Execute();
        }
    }
}