using System.Data.Common;
using System.Data.SQLite;

namespace Kiwi.Json.DocumentDatabase.Sqlite
{
    public class MemoryDatabase : AbstractDatabase, ITxFactory
    {
        public MemoryDatabase()
        {
            Connection = new SQLiteConnection(@"Data Source=:memory:");
            Connection.Open();

            EnforceSchema();
        }

        protected SQLiteConnection Connection { get; private set; }

        protected override ITxFactory TxFactory
        {
            get { return this; }
        }

        #region ITxFactory Members

        ITx ITxFactory.CreateTransaction()
        {
            return new Tx(Connection);
        }

        #endregion

        #region Nested type: Tx

        private class Tx : ITx
        {
            private readonly SQLiteConnection _connection;
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
                    _transaction = null;
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

        #endregion
    }
}