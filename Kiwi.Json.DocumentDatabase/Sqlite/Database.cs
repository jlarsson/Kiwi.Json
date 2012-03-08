using System.Data.Common;
using System.Data.SQLite;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class Database: AbstractDatabase, ITxFactory
    {
        private readonly string _filePath;

        public Database(string filePath)
        {
            _filePath = filePath;
            EnforceSchema();
        }

        protected override ITxFactory TxFactory
        {
            get { return this; }
        }

        public ITx CreateTransaction()
        {
            return new Tx(new SQLiteConnection(@"Data Source="+ _filePath));
        }

        private class Tx : ITx
        {
            private SQLiteConnection _connection;
            private SQLiteTransaction _transaction;
            private bool _isDisposed;

            public Tx(SQLiteConnection connection)
            {
                _connection = connection;
            }

            #region ITx Members

            public DbCommand CreateCommand()
            {
                if (_isDisposed)
                {
                    throw new InvalidSessionStateException();
                }

                if (_transaction == null)
                {
                    _connection.Open();
                    
                    _transaction = _connection.BeginTransaction();
                }
                var command = _connection.CreateCommand();
                command.Transaction = _transaction;
                return command;
            }

            public void Commit()
            {
                if (_isDisposed)
                {
                    throw new InvalidSessionStateException();
                }
                _isDisposed = true;
                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    _transaction = null;

                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;

                }
            }

            public void Rollback()
            {
                if (_isDisposed)
                {
                    throw new InvalidSessionStateException();
                }
                _isDisposed = true;
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Rollback();
                }
            }

            #endregion
        }
    }
}