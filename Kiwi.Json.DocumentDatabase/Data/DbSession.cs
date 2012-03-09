using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Kiwi.Json.DocumentDatabase.Data
{
    public class DbSession : IDbSession
    {
        private DbConnection _connection;
        private readonly bool _isTransientConnection;
        private DbTransaction _transaction;
        private bool _isDisposed;

        public DbSession(DbConnection connection, bool isTransientConnection)
        {
            _connection = connection;
            _isTransientConnection = isTransientConnection;
        }

        #region IDbSession Members

        public void Execute(IDatabaseCommand command)
        {
            ExecuteCommand(command, c => c.ExecuteNonQuery());
        }

        public IEnumerable<T> Query<T>(IDatabaseCommand command, Func<IAccessor, T> map)
        {
            return ExecuteCommand(command, c =>
                                               {
                                                   var dt = new DataTable();
                                                   using (var reader = c.ExecuteReader())
                                                   {
                                                       dt.Load(reader);
                                                   }
                                                   return from row in dt.Rows.OfType<DataRow>()
                                                          let accessor = new DataRowAccessor(row)
                                                          select map(accessor);
                                               });
        }

        public IDatabaseCommand CreateSqlCommand(string sql)
        {
            return new DbCommandDatabaseCommand(sql, this);
        }

        private void Dispose(Action<DbTransaction> shutDownTransaction)
        {
            if (_isDisposed)
            {
                throw new InvalidSessionStateException();
            }
            _isDisposed = true;
            if (_transaction != null)
            {
                shutDownTransaction(_transaction);
                _transaction.Dispose();
                _transaction = null;
            }
            if ((_connection != null) && _isTransientConnection)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Rollback();
            }
        }

        public void Commit()
        {
            Dispose(tx => tx.Commit());
        }

        public void Rollback()
        {
            Dispose(tx => tx.Rollback());
        }

        #endregion

        protected virtual T ExecuteCommand<T>(IDatabaseCommand command, Func<DbCommand, T> executeCommand)
        {
            if (_isDisposed)
            {
                throw new InvalidSessionStateException();
            }

            if (_transaction == null)
            {
                if (_isTransientConnection)
                {
                    _connection.Open();
                }
                _transaction = _connection.BeginTransaction();
            }

            try
            {
                using (var dbcommand = _connection.CreateCommand())
                {
                    dbcommand.Transaction = _transaction;
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
                Rollback();
                throw;
            }
        }
    }
}